import { CustomTooltip } from "@/components/utils/customTooltip";
import { generateColorMap, generateDurationData, generatePieData } from "@/lib/utils/appFunctions";
import { Cell, Bar, BarChart, ResponsiveContainer, Tooltip, XAxis, YAxis } from "recharts";

type BarEntry = {
    name: string;
    duration: number;
};

type BarChartWidgetProps = {
    data: BarEntry[];
    colorMap?: Record<string, string>;
};

export const BarChartWidget: React.FC<BarChartWidgetProps> = ({
    data,
    colorMap
  }) => {
  
    const internalColorMap = colorMap ?? generateColorMap(data);

    return (
        <ResponsiveContainer width="100%" height="100%">
            <BarChart data={data}>
                <XAxis dataKey="name" />
                <YAxis />
                <Tooltip content={CustomTooltip} />
                <Bar
                    dataKey="value"
                    radius={[10, 0, 10, 0]}
                    fillOpacity={1}
                >
                    {data.map((entry, index) => (
                        <Cell key={`cell-bar-${index}`} fill={internalColorMap[entry.name]} />
                    ))}
                </Bar>
            </BarChart>
        </ResponsiveContainer>
    );
};