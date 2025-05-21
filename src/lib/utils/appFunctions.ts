import dayjs from 'dayjs';
import { EducationFormData } from '../schemas/educationSchema';
import {
    Menu, Home, Info, LayoutDashboard, Book, Briefcase, Folder, BadgePercent,
    Languages, PenSquare, MessageSquare, Settings, LogOut
} from 'lucide-react';

export function transformPayload<T extends object>(obj: T): T {
    return Object.fromEntries(
        Object.entries(obj).map(([key, value]) => 
            [key, value === '' ? null : value]
        )
    ) as T;
};

export const generatePieData = <T extends Record<string, any>>(
    list: T[],
    key: string | Record<string, string | string[]>
) => {
    const counts = list.reduce((acc: Record<string, number>, item) => {
        const field = extractValue(item, key) as string;
        if (!field) return acc;

        acc[field] = (acc[field] || 0) + 1;
        return acc;
    }, {});

    return Object.entries(counts).map(([name, value]) => ({ name, value }));
};

export const generateDurationData = <T extends Record<string, any>>(
    list: T[],
    nameKey:  string | Record<string, string | string[]>,
    startDateKey: keyof T = 'startDate',
    endDateKey: keyof T = 'endDate',
    unit: dayjs.ManipulateType = 'month'
): { name: string; duration: number }[] => {
    return list.map(item => {
        const start = dayjs(item[startDateKey]);
        const end = item[endDateKey] ? dayjs(item[endDateKey]) : dayjs();
        const name = extractValue(item, nameKey);
        return {
            name: name ?? 'Unknown',
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

export const getSelectedOption = (options: {label: string; value: string}[], value: string | undefined) =>
    options.find(opt => opt.value === value);

export const getNavLinks = (username?: string | undefined, role?: 'owner' | 'client' | 'admin' ) => {
    if (!username) return [{ href: '/', label: 'Home', icon: Home }];

    if (role === 'client' || !role) return [
        { href: '/', label: 'Home', icon: Home },
        { href: `/client/${username}/dashboard`, label: 'Dashboard', icon: LayoutDashboard }
    ];

    if (role === 'owner') return [
        { href: '/', label: 'Home', icon: Home },
        { href: `/owner/${username}/dashboard`, label: 'Dashboard', icon: LayoutDashboard },
        { href: `/owner/${username}/education`, label: 'Education', icon: Book },
        { href: `/owner/${username}/experience`, label: 'Experience', icon: Briefcase },
        { href: `/owner/${username}/project`, label: 'Projects', icon: Folder },
        { href: `/owner/${username}/skill`, label: 'Skills', icon: BadgePercent },
        { href: `/owner/${username}/language`, label: 'Languages', icon: Languages },
        { href: `/owner/${username}/blog-post`, label: 'Blog Post', icon: PenSquare },
        { href: `/owner/${username}/message`, label: 'Messages', icon: MessageSquare },
        { href: `/owner/${username}/setting`, label: 'Settings', icon: Settings },
        { href: `/owner/${username}/logout`, label: 'Logout', icon: LogOut },
    ];

    return [{ href: '/', label: 'Home', icon: Home }];
}

export const getClientLink = (): string | null => {
    if (typeof window === 'undefined') return null;

    const { origin, pathname } = window.location;
    const parts = pathname.split('/');

    if (parts.length > 1) {
        parts[1] = 'client';
    }

    const modifiedPath = parts.join('/');
    return `${origin}${modifiedPath}`;
};
