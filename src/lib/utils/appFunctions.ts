import dayjs from 'dayjs';
import { EducationFormData } from '../schemas/educationSchema';

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

export const extractValue = (
    item: any,
    key: string | Record<string, string | string[]>
): any => {
    if (typeof key === 'string') {
        return item[key];
    }

    if (typeof key === 'object') {
        const [parentKey, nestedKeys] = Object.entries(key)[0];
        const parent = item[parentKey];

        if (!parent) return undefined;

        if (typeof nestedKeys === 'string') {
            return parent[nestedKeys];
        }

        if (Array.isArray(nestedKeys)) {
            return nestedKeys
                .map(k => parent?.[k])
                .filter(Boolean) // remove undefined/null
                .join(' '); // or customize this!
        }
    }

    return undefined;
};

export const mapEducationToForm = (educationFromDb: any): EducationFormData => ({
    ...educationFromDb,
    startDate: educationFromDb.startDate?.slice(0, 10),
    endDate: educationFromDb.endDate?.slice(0, 10),
    LKP_InstitutionID: educationFromDb.institution?.id ?? '',
    LKP_DegreeID: educationFromDb.degree?.id ?? '',
    LKP_FieldOfStudyID: educationFromDb.fieldOfStudy?.id ?? '',
});
