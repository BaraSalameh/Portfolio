import dayjs from 'dayjs';
import { EducationFormData } from '../schemas/educationSchema';
import {
    Menu, Home, Info, LayoutDashboard, Book, Briefcase, Folder, BadgePercent,
    Languages, PenSquare, MessageSquare, Settings, LogOut
} from 'lucide-react';
import { string } from 'zod';
import { ProjectTechnologyFormData } from '../schemas/projectTechnologyScehma';
import { UserLanguageFormData } from '../schemas';

export function transformPayload<T extends object>(obj: T): T {
    return Object.fromEntries(
        Object.entries(obj).map(([key, value]) => 
            [key, value === '' ? null : value]
        )
    ) as T;
};

const normalizeFieldValue = (value: any): string[] => {
    if (Array.isArray(value)) return value.filter(Boolean);
    if (value) return [value];
    return [];
};


export const generatePieData = <T extends Record<string, any>>(
    list: T[],
    key: string | Record<string, string | string[]>
) => {
    const counts = new Map<string, number>();

    list.forEach(item => {
        const names = normalizeFieldValue(extractValue(item, key));

        names.forEach(name => {
            counts.set(name, (counts.get(name) ?? 0) + 1);
        });
    });

    return Array.from(counts.entries()).map(([name, value]) => ({ name, value }));
};



export const generateDurationData = <T extends Record<string, any>>(
    list: T[],
    nameKey: string | Record<string, string | string[]>,
    startDateKey: keyof T = 'startDate',
    endDateKey: keyof T = 'endDate',
    unit: dayjs.ManipulateType = 'month'
): { name: string; value: number }[] => {
    const durations = new Map<string, number>();

    list.forEach(item => {
        const start = item[startDateKey] ? dayjs(item[startDateKey]) : null;
        const end = item[endDateKey] ? dayjs(item[endDateKey]) : dayjs();
        const value = start ? end.diff(start, unit) : null;

        const names = normalizeFieldValue(extractValue(item, nameKey)) || ['Unknown'];

        names.forEach(name => {
            const total = durations.get(name) ?? 0;
            durations.set(name, total + (value !== null ? value : 1));
        });
    });

    return Array.from(durations.entries()).map(([name, value]) => ({ name, value }));
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
        return item?.[key];
    }

    const [parentKey, nestedKeys] = Object.entries(key)[0];
    const parent = item?.[parentKey];

    if (!parent) return undefined;

    // Case: parent is an array and nestedKeys is a string
    if (Array.isArray(parent) && typeof nestedKeys === 'string') {
        return parent.map(child => child?.[nestedKeys]).filter(Boolean);
    }

    // Case: parent is an object and nestedKeys is a string
    if (typeof nestedKeys === 'string') {
        return parent?.[nestedKeys];
    }

    // Case: parent is an object and nestedKeys is an array
    if (Array.isArray(nestedKeys)) {
        return nestedKeys
            .map(k => parent?.[k])
            .filter(Boolean)
            .join(' | ');
    }

    return undefined;
};


export const mapEducationToForm = (educationFromDb: any): EducationFormData => {
    const result = educationFromDb
        ?   {
                ...educationFromDb,
                startDate: educationFromDb.startDate?.slice(0, 10),
                endDate: educationFromDb.endDate?.slice(0, 10),
                LKP_InstitutionID: educationFromDb.institution?.id ?? '',
                LKP_DegreeID: educationFromDb.degree?.id ?? '',
                LKP_FieldOfStudyID: educationFromDb.fieldOfStudy?.id ?? '',
            }
        : null;
    return result;
};

export const mapProjectTechnologyToForm = (projectTechnologyFromDb: any): EducationFormData => {
    const result = projectTechnologyFromDb
        ?   {
                ...projectTechnologyFromDb,
                lstTechnologies: projectTechnologyFromDb.lstTechnologies?.map(
                    (pt: any) => pt.id
                ) ?? []
            }
        : null;
    return result;
};

export const mapUserLanguageToForm = (userLanguageFromDb: any): UserLanguageFormData => {
    const result = userLanguageFromDb
        ?   {
                ...userLanguageFromDb,
                lkP_LanguageID: userLanguageFromDb.language.id,
                lkP_LanguageProficiencyID: userLanguageFromDb.languageProficiency.id
            }
        : null;
    return result;
}

export const getSelectedOption = (
    options: { label: string; value: string }[],
    value: string | string[] | undefined
) => {
    if (!value) return Array.isArray(value) ? [] : undefined;

    return Array.isArray(value)
        ? options.filter(opt => value.includes(opt.value))
        : options.find(opt => opt.value === value);
};


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