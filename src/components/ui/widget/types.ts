import { WidgetCardVariantProps, WidgetListVariantProps } from "@/styles/widget";

export interface WidgetCardProps extends WidgetCardVariantProps {
    isLoading?: boolean;
    header?: {
        title?: string;
        icon?: any;
    };
    items: Record<string, any>[];
    list?: ListItemConfig[];
    pie?: {
        title?: string;
        groupBy: string | Record<string, string | string[]>;
    };
    bar?: {
        title?: string;
        groupBy: string | Record<string, string | string[]>;
        durationKeys?: {
            start?: string;
            end?: string;
        };
    };
    create?: {
        title?: string;
        subTitle?: string;
        form?: React.ReactNode;
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
    className?: string;
}

export interface WidgetChartsProps {
    pieData: any;
    durationData: any;
    pieTitle?: string;
    barTitle?: string;
    colorMap: Record<string, string>;
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
}

type CahrtEntry = {
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