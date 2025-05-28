import { CustomTooltip } from "@/components/utils";
import { ResponsiveContainer, Tooltip, RadarChart, PolarGrid, PolarAngleAxis, PolarRadiusAxis, Radar, Legend } from "recharts";
import { ChartWidgetProps } from "./types";

export const RadarChartWidget = ({
    data
}: ChartWidgetProps) => {

    return (
        <ResponsiveContainer width="100%" height="100%">
            <RadarChart cx="50%" cy="50%" outerRadius="80%" data={data}>
                <PolarGrid />
                <PolarAngleAxis dataKey="name" />
                <Radar
                    dataKey="value"
                    stroke="#3B82F6"
                    fill="#F97316"
                    fillOpacity={0.7}
                />
                <Tooltip content={CustomTooltip} />
            </RadarChart>
      </ResponsiveContainer>
    );
};