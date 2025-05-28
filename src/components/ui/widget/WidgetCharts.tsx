import { Paragraph } from '..';
import { PieChartWidget, BarChartWidget } from '.';
import { WidgetChartsProps } from './types';
import { RadarChartWidget } from './RadarChart';

export const WidgetCharts = ({ pieData, durationData, radarData, pieTitle, barTitle, radarTitle, colorMap }: WidgetChartsProps) => (
    <div
        className={`w-full grid gap-4 
        ${pieData?.length && durationData?.length && radarData?.length
            ? 'grid-cols-3 auto-rows-min'
            : pieData?.length && durationData?.length
            ? 'grid-cols-3'
            : pieData?.length && radarData?.length
            ? 'grid-cols-3'
            : durationData?.length && radarData?.length
            ? 'grid-cols-3'
            : 'grid-cols-1'
        }`}
    >
        {pieData?.length > 0 && (
            <div
                className={`h-64 space-y-3
                ${durationData?.length && radarData?.length
                    ? 'col-span-1'
                    : durationData?.length || radarData?.length
                    ? 'col-span-1'
                    : 'col-span-3'
                }`}
            >
                <Paragraph size="md" position="center">{pieTitle ?? 'Distribution'}</Paragraph>
                <PieChartWidget data={pieData} colorMap={colorMap} />
            </div>
        )}

        {durationData?.length > 0 && (
            <div
                className={`h-64 space-y-3
                ${pieData?.length && radarData?.length
                    ? 'col-span-2'
                    : pieData?.length
                    ? 'col-span-2'
                    : radarData?.length
                    ? 'col-span-1'
                    : 'col-span-3'
                }`}
            >
                <Paragraph size="md" position="center">{barTitle ?? 'Duration (in months)'}</Paragraph>
                <BarChartWidget data={durationData} colorMap={colorMap} />
            </div>
        )}

        {radarData?.length > 0 && (
            <div
                className={`h-64 space-y-3
                ${pieData?.length && durationData?.length
                    ? 'col-span-3'
                    : pieData?.length || durationData?.length
                    ? 'col-span-2'
                    : 'col-span-3'
                }`}
            >
                <Paragraph size="md" position="center">{radarTitle ?? 'Distribution'}</Paragraph>
                <RadarChartWidget data={radarData} colorMap={colorMap} />
            </div>
        )}
    </div>

    // <div className={`w-full grid grid-cols-1 ${pieData && durationData.length > 0 ? 'sm:grid-cols-3' : ''}`}>
    //     {pieData?.length > 0 && (
    //         <div className="h-64">
    //             <Paragraph size="md" position="center">{pieTitle ?? 'Distribution'}</Paragraph>
    //             <PieChartWidget data={pieData} colorMap={colorMap} />
    //         </div>
    //     )}
    //     {durationData?.length > 0 && (
    //         <div className="h-64 col-span-2 space-y-3">
    //             <Paragraph size="md" position="center">{barTitle ?? 'Duration (in months)'}</Paragraph>
    //             <BarChartWidget data={durationData} colorMap={colorMap} />
    //         </div>
    //     )}
    //     {radarData?.length > 0 && (
    //         <div className={`h-64 col-span-2 space-y-3`}>
    //             <Paragraph size="md" position="center">{radarTitle ?? 'Distribution'}</Paragraph>
    //             <RadarChartWidget data={radarData} colorMap={colorMap} />
    //         </div>
    //     )}
    // </div>
);
