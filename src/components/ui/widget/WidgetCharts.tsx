import { Paragraph } from '../Paragraph';
import { PieChartWidget } from './PieChart';
import { BarChartWidget } from './BarChart';

interface WidgetChartsProps {
    pieData: any;
    durationData: any;
    pieTitle?: string;
    barTitle?: string;
    colorMap: Record<string, string>;
}

export const WidgetCharts: React.FC<WidgetChartsProps> = ({ pieData, durationData, pieTitle, barTitle, colorMap }) => (
    <div className={`w-full grid grid-cols-1 ${pieData && durationData.length > 0 ? 'sm:grid-cols-3' : ''}`}>
        {pieData && (
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
