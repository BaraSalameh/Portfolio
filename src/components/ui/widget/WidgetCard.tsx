'use client';

import { widgetCard, WidgetCardVariantProps } from '@/styles/widget';
import { cn } from '@/components/utils/cn';
import { Header } from '@/components/shared/Header';
import { Main } from '@/components/shared/Main';
import { Paragraph } from '../Paragraph';
import { List } from '../List';
import ResponsiveIcon from '../ResponsiveIcon';
import { generateColorMap, generateDurationData, generatePieData } from '@/lib/utils/appFunctions';
import { useState, useMemo, useEffect } from 'react';
import { ListItemConfig, WidgetList } from './WidgetList';
import { WidgetCharts } from './WidgetCharts';
import { WidgetModal } from './WidgetModal';
import { CUDModal } from '../CUDModal';
import React from 'react';
import { Scroll } from 'lucide-react';

interface WidgetCardProps extends WidgetCardVariantProps {
    header?: {
        title?: string;
        icon?: any;
    };
    items: Record<string, any>[];
    list?: ListItemConfig[];
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
        form?: React.ReactNode;
    };
    update?: {
        title?: string;
        subTitle?: string;
        form?: React.ReactNode;
    };
    del?: {
        title?: string;
        subTitle?: string;
        message?: string;
        onDelete: (id: string) => any;
    };
    details?: ListItemConfig[];
    sortable?: boolean;
    className?: string;
}

export const WidgetCard: React.FC<WidgetCardProps> = ({
    header,
    items,
    list,
    pie,
    bar,
    create,
    update,
    del,
    details,
    sortable = true,
    className
}) => {
    if (!Array.isArray(items) || items.length === 0 || (!header && !list && !pie && !bar)) return null;

    const [draggable, setDraggable] = useState(sortable);
    const [openModal, setOpenModal] = useState(false);
    const [item, setItem] = useState<Record<string, any> | undefined>();
    const handleModal = (item: Record<string, any>) => {
        setOpenModal(true);
        setItem(item);
    };

    const clickable = (update || del || details) ? handleModal : undefined;

    const pieData = useMemo(() => pie ? generatePieData(items, pie.groupBy) : null, [items, pie]);
    const durationData = useMemo(
        () => bar ? generateDurationData(items, bar.groupBy, bar.durationKeys?.start || 'startDate', bar.durationKeys?.end || 'endDate') : [],
        [items, bar]
    );
    const colorMap = useMemo(() => generateColorMap(pieData ?? durationData), [pieData, durationData]);

    useEffect(() => {
        console.log(draggable);
    }, [draggable]);
    return (
        <React.Fragment>
            <section className={cn(widgetCard({}), className)}>
                {header && (header.icon || header.title) && (
                    <Header itemsX="between" paddingX="xs" paddingY="xs">
                        <Paragraph size="lg" space="xs">
                            {header.icon && <ResponsiveIcon icon={header.icon} />}
                            {header.title}
                        </Paragraph>
                        {(create || sortable) &&
                            <div className='flex space-x-3'>
                                {sortable && 
                                    <CUDModal as='none' icon={Scroll} subTitle='Drag and Drop'>
                                        <Paragraph> {`Drag and drop is ${draggable ? 'ON' : 'OFF'}`} </Paragraph>
                                    </CUDModal>
                                    // <button><ResponsiveIcon icon={header.icon} onClick={() => setDraggable(!draggable)}/></button>
                                }
                                {create && (
                                    <CUDModal title={create.title} subTitle={create.subTitle}>
                                        {create.form}
                                    </CUDModal>
                                )}
                            </div>
                        }
                    </Header>
                )}

                {(pie || bar) && (
                    <Main paddingX="none" paddingY="md">
                        <WidgetCharts pieData={pieData} durationData={durationData} pieTitle={pie?.title} barTitle={bar?.title} colorMap={colorMap} />
                    </Main>
                )}

                {list && (
                    <Main paddingX="none" paddingY="none">
                        <List size="md" as="none" className="w-full">
                            <WidgetList items={items} list={list} onItemClick={clickable} />
                        </List>
                    </Main>
                )}
            </section>

            <WidgetModal
                isOpen={openModal}
                onClose={() => setOpenModal(false)}
                item={item}
                update={update}
                del={del}
                details={details}
                className={className}
            />
        </React.Fragment>
    );
};
