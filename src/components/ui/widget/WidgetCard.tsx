'use client';

import { generateColorMap, generateDurationData, generatePieData } from '@/lib/utils/appFunctions';
import { Header } from '@/components/shared/Header';
import { Main } from '@/components/shared/Main';
import { Paragraph } from '../Paragraph';
import { PieChartWidget } from './PieChart';
import { BarChartWidget } from './BarChart';
import { List } from '../List';
import ResponsiveIcon from '../ResponsiveIcon';
import dayjs from 'dayjs';
import { widgetCard, WidgetCardVariantProps } from '@/styles/widgetCard';
import { cn } from '@/components/utils/cn';
import { CUDModal } from '../CUDModal';
import { useState } from 'react';
import { BlurBackGround } from '../BlurBackGround';
import { X } from 'lucide-react';
import React from 'react';
import { Button } from '../Button';
import Image from "next/image";

interface WidgetCardProps extends WidgetCardVariantProps {
    header?: {
        title?: string;
        icon?: any;
    };
    items: Record<string, any>[];
    list?: {
        icon?: any;
        leftKey: string;
        between?: string;
        rightKey?: string;
        size?: 'lg' | 'md' | 'sm' | null;
        isTime?: boolean;
    }[];
    pie?: {
        title?: string;
        groupBy: string;
    };
    bar?: {
        title?: string;
        groupBy: string;
        durationKeys?: {
            start?: string;
            end?: string;
        };
    };
    create?: {
        title?: string;
        subTitle?: string;
        form?: React.ReactNode
    };
    update?: {
        title?: string;
        subTitle?: string;
        form?: React.ReactNode
    };
    del?: {
        title?: string;
        subTitle?: string;
        message?: string;
        CBDelete: (id: string) => any;
    };
    className?: string;
};

export const WidgetCard: React.FC<WidgetCardProps> = ({
    header,
    items,
    list,
    pie,
    bar,
    create,
    update,
    del,
    className
}) => {
    if (!Array.isArray(items) || items.length === 0) return null;
    if (!header && !list && !pie && !bar) return null;

    const [openModal, setOpenModal] = useState(false);
    const [itemId, setItemId] = useState<string | undefined>();
    const handleModal = (id: string) => {
        setOpenModal(true);
        setItemId(id);
    }
    const clickable = update ? handleModal : () => {};

    // Chart data generation
    const pieData = pie ? generatePieData(items, pie.groupBy) : null;
    const durationData = bar
        ? generateDurationData(
            items,
            bar.groupBy,
            bar.durationKeys?.start || 'startDate',
            bar.durationKeys?.end || 'endDate'
        )
        : [];
    const colorMap = generateColorMap(pieData ?? durationData);

    const renderListItem = (item: Record<string, any>, idx: number) => (
            <li key={item.id ?? idx}  className={`${cn(widgetCard({ intent: 'list' }), className)}`} onClick={() => clickable(item.id)}>
                {list!.map((cfg, index) => {
                    const leftVal = cfg.isTime ? dayjs(item[cfg.leftKey]).format('MMM YYYY') : item[cfg.leftKey];
                    const rightVal = cfg.isTime
                        ? cfg.rightKey && item[cfg.rightKey]
                            ? dayjs(item[cfg.rightKey]).format('MMM YYYY')
                            : 'Present'
                        : cfg.rightKey
                            ? item[cfg.rightKey]
                            : '';

                    return (
                        <Paragraph key={index} size={cfg.size} className='gap-3'>
                            {cfg.icon && <ResponsiveIcon icon={cfg.icon} />}
                            {leftVal} {cfg.between ?? ''} {rightVal}
                        </Paragraph>
                    );
                })}
            </li>
    );

    return (
        <>
        <section  className={`${cn(widgetCard({}), className)}`}>
            {/* Header */}
            {header && (header.icon || header.title) && (
                <Header itemsX='between' paddingX="xs" paddingY="xs">
                    <Paragraph size='lg' space='xs'>
                        {header.icon && <ResponsiveIcon icon={header.icon} />}
                        {header.title && header.title}
                    </Paragraph>
                    {create && 
                        <CUDModal title={create.title} subTitle={create.subTitle}>
                            {create.form}
                        </CUDModal>
                    }
                </Header>
            )}

            {/* Pie and Bar Chart */}
            {(pie || bar) && (
                <Main paddingX="none" paddingY="md">
                    <div className={`w-full grid grid-cols-1 ${pie && bar ? 'sm:grid-cols-3' : ''}`}>
                        {pie && pieData && (
                            <div className="h-64">
                                <Paragraph size="md" position="center">{pie.title ?? 'Distribution'}</Paragraph>
                                <PieChartWidget data={pieData} colorMap={colorMap} />
                            </div>
                        )}

                        {bar && durationData.length > 0 && (
                            <div className="h-64 col-span-2 space-y-3">
                                <Paragraph size="md" position="center">{bar.title ?? 'Duration (in months)'}</Paragraph>
                                <BarChartWidget data={durationData} colorMap={colorMap} />
                            </div>
                        )}
                    </div>
                </Main>
            )}

            {/* List */}
            {list && (
                <Main paddingX="none" paddingY="none">
                    <List size="md" as="none" className="w-full">
                        {items.map(renderListItem)}
                    </List>
                </Main>
            )}
        </section>
        {openModal && (
            <BlurBackGround intent='sm'>
                <div className={`${cn(widgetCard({}), className)}`}>
                    <Header itemsX='between' paddingX="xs" paddingY="xs">
                        
                        {(update || del) &&
                            <>
                                {update &&
                                    <CUDModal as='update' title={update.title} subTitle={update.subTitle}>
                                        {React.isValidElement(update.form)
                                            ? React.cloneElement(update.form as React.ReactElement<{ onClose: () => void, id: string }>, {
                                                    onClose: () => setOpenModal(false),
                                                    id: itemId
                                                })
                                            : update.form
                                        }
                                    </CUDModal>
                                }
                                {del &&
                                    <CUDModal as='delete' title={del.title} subTitle={del.subTitle} CBRedux={del.CBDelete} idToDelete={itemId}>
                                        {del.message}
                                    </CUDModal>
                                }
                                </>
                        }
                        <X className='cursor-pointer' onClick={() => setOpenModal(false)} />
                    </Header>
                </div>
            </BlurBackGround>
        )}
        </>
    );
};
