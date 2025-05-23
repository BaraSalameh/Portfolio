import { Paragraph } from '..';
import { PieChartWidget, BarChartWidget } from '.';
import { WidgetChartsProps } from './types';

export const WidgetCharts = ({ pieData, durationData, pieTitle, barTitle, colorMap }: WidgetChartsProps) => (
    <div className={`w-full grid grid-cols-1 ${pieData && durationData.length > 0 ? 'sm:grid-cols-3' : ''}`}>
        {pieData.length > 0 && (
            <div className="h-64">
                <Paragraph size="md" position="center">{pieTitle ?? 'Distribution'}</Paragraph>
                <PieChartWidget data={pieData} colorMap={colorMap} />
            </div>
        )}
        {durationData.length > 0 && (
            <div className="h-64 col-span-2 space-y-3">
                <Paragraph size="md" position="center">{barTitle ?? 'Duration (in months)'}</Paragraph>
                <BarChartWidget data={durationData} colorMap={colorMap} />
            </div>
        )}
    </div>
);
