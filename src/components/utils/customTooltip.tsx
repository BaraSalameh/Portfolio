import { Paragraph } from "../ui/Paragraph";

export const CustomTooltip = ({ active, payload, label } : any) => {
    if (active && payload && payload.length) {
        return (
            <div className="bg-green-700 p-4">
                <Paragraph >{label}</Paragraph>
                {payload.map((item: any, index: any)  => (
                    <Paragraph size='sm' key={index}>
                        {item.name}: {item.value}
                    </Paragraph>
                ))}
            </div>
        );
    }
    return null;
};