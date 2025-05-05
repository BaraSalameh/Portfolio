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

type WidgetCardProps = {
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
};

export const WidgetCard: React.FC<WidgetCardProps> = ({ header, items, list, pie, bar }) => {
    if (!Array.isArray(items) || items.length === 0) return null;
    if (!header && !list && !pie && !bar) return null;

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
            <li key={item.id ?? idx} className="bg-green-700 p-4 rounded-xl shadow-sm space-y-2">
                {list!.map((cfg, index) => {
                    const leftVal = cfg.isTime ? dayjs(item[cfg.leftKey]).format('MMM YYYY') : item[cfg.leftKey];
                    const rightVal = cfg.isTime
                        ? cfg.rightKey
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
        <section className="bg-green-900 p-4 rounded-2xl space-y-4">
            {/* Header */}
            {header && (header.icon || header.title) && (
                <Header paddingX="xs" paddingY="xs" space="sm">
                    {header.icon && <ResponsiveIcon icon={header.icon} />}
                    {header.title && <Paragraph size="lg">{header.title}</Paragraph>}
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
    );
};
