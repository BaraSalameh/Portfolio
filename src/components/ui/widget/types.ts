import { WidgetCardVariantProps, WidgetListVariantProps } from "@/styles/widget";
import { LucideIcon } from "lucide-react";

export type PieChartProps = {
    title?: string;
    groupBy: string | Record<string, string | string[]>;
} 

export type BarChartProps = {
    title?: string;
    groupBy?: string | Record<string, string | string[]>;
    durationKeys?: {
        start?: string;
        end?: string;
    };
    customData?: CahrtEntry[];
}

export type RadarChartProps = {
    title?: string;
    groupBy?: string | Record<string, string | string[]>;
    customData?: CahrtEntry[];
}

export interface WidgetCardProps extends WidgetCardVariantProps {
    isLoading?: boolean;
    header?: {
        title?: string;
        icon?: any;
    };
    items: Record<string, any>[];
    list?: ListItemConfig[];
    pie?: PieChartProps;
    bar?: BarChartProps;
    radar?: RadarChartProps;
    create?: {
        title?: string;
        subTitle?: string;
        form?: React.ReactNode;
        icon?: LucideIcon;
    };
    update?: {
        title?: string;
        subTitle?: string;
        form?: React.ReactNode;
    };
    del?: {
        title?: string;
        subTitle?: string;
        message?: string;
        onDelete: (id: string) => any;
    };
    details?: ListItemConfig[];
    onSort?: (lstIds: string[]) => any;
    onModalAction?: (id: string) => any;
    className?: string;
}

export interface WidgetChartsProps {
    items?: Record<string, any>[];
    pie?: PieChartProps;
    bar?: BarChartProps;
    radar?: RadarChartProps;
}

export interface ListItemConfig {
    icon?: any;
    leftKey?: string | Record<string, string | string[]>;
    between?: string;
    rightKey?: string | Record<string, string | string[]>;
    size?: 'lg' | 'md' | 'sm' | null;
    isTime?: boolean;
}

export interface WidgetListProps extends WidgetListVariantProps {
    items: Record<string, any>[];
    list: ListItemConfig[];
    onItemClick?: (item: any) => void;
    className?: string;
    sort?: {
        sortable: boolean;
        onSort?: (lstIds: string[]) => any;
    }
}

export interface WidgetModalProps {
    isLoading?: boolean;
    isOpen: boolean;
    onClose: () => void;
    item?: Record<string, any>;
    update?: {
        title?: string;
        subTitle?: string;
        form?: React.ReactNode;
    };
    del?: {
        title?: string;
        subTitle?: string;
        message?: string;
        onDelete: (id: string) => any;
    };
    details?: ListItemConfig[];
    className?: string;
    onAction?: (id: string) => any;
}

export type CahrtEntry = {
    name: string;
    value: number;
}

export type ChartWidgetProps = {
    data: CahrtEntry[];
    colorMap?: Record<string, string>;
}

export interface SortableItemProps {
    id: string;
    children: React.ReactNode;
}