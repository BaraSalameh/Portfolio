import dayjs from 'dayjs';

export function transformPayload<T extends object>(obj: T): T {
    return Object.fromEntries(
        Object.entries(obj).map(([key, value]) => 
            [key, value === '' ? null : value]
        )
    ) as T;
};

export const generatePieData = <T extends Record<string, any>>(
    list: T[],
    key: keyof T
) => {
    const counts = list.reduce((acc: Record<string, number>, item) => {
        const field = item[key] as string;
        acc[field] = (acc[field] || 0) + 1;
        return acc;
    }, {});

    return Object.entries(counts).map(([name, value]) => ({ name, value }));
};

export const generateDurationData = <T extends Record<string, any>>(
    list: T[],
    nameKey: keyof T,
    startDateKey: keyof T = 'startDate',
    endDateKey: keyof T = 'endDate',
    unit: dayjs.ManipulateType = 'month'
): { name: string; duration: number }[] => {
    return list.map(item => {
        const start = dayjs(item[startDateKey]);
        const end = item[endDateKey] ? dayjs(item[endDateKey]) : dayjs();
        return {
            name: item[nameKey] as string,
            duration: end.diff(start, unit),
        };
    });
};

export const generateColorMap = (
    data: { name: string }[],
    colors: string[] = ['#F97316', '#3B82F6', '#10B981', '#EAB308', '#6366F1']
): Record<string, string> => {
    return data.reduce((acc, item, index) => {
        acc[item.name] = colors[index % colors.length];
        return acc;
    }, {} as Record<string, string>);
};
