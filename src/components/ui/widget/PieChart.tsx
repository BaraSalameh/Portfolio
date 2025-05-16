import { CustomTooltip } from "@/components/utils/customTooltip";
import { generateColorMap } from "@/lib/utils/appFunctions";
import { Cell, Pie, PieChart, ResponsiveContainer, Tooltip } from "recharts";

type PieEntry = {
    name: string;
    value: number;
};

type PieChartWidgetProps = {
    data: PieEntry[];
    colorMap?: Record<string, string>;
};

export const PieChartWidget: React.FC<PieChartWidgetProps> = ({
    data,
    colorMap
  }) => {

    const internalColorMap = colorMap ?? generateColorMap(data);
  
    return (
        <ResponsiveContainer width="100%" height={300}>
            <PieChart>
                <Pie
                    data={data}
                    dataKey="value"
                    nameKey="name"
                    cx="50%"
                    cy="50%"
                    outerRadius={50}
                    label
                >
                    {data.map((entry, index) => (
                        <Cell key={`cell-${index}`} fill={internalColorMap[entry.name]} />
                    ))}
                </Pie>
                <Tooltip content={CustomTooltip} />
            </PieChart>
        </ResponsiveContainer>
    );
};