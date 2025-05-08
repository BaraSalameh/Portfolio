import { WidgetCardVariantProps, WidgetListVariantProps } from "@/styles/widget";

export interface WidgetCardProps extends WidgetCardVariantProps {
    header?: {
        title?: string;
        icon?: any;
    };
    items: Record<string, any>[];
    list?: ListItemConfig[];
    pie?: {
        title?: string;
        groupBy: string;
    };
    bar?: {
        title?: string;
        groupBy: string;
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
    leftKey: string;
    between?: string;
    rightKey?: string;
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
