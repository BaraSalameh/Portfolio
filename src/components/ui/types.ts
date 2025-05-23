import { ListVariantProps, BlurBackgroundVariantProps, ParagraphVariantProps } from "@/styles";
import { LucideIcon } from "lucide-react";
import { InputHTMLAttributes } from "react";

export interface BlurBackgroundProps extends BlurBackgroundVariantProps {
    children: React.ReactNode;
    className?: string;
    onClick?: () => void;
}

type As = 'create' | 'update' | 'delete' | 'none';

export interface CUDProps extends InputHTMLAttributes<HTMLInputElement> {
    isLoading?: boolean;
    idToDelete?: string;
    onAction?: (id: string) => any;
    onClose?: () => void;
    as?: As;
    title?: string;
    subTitle?: string;
    children?: React.ReactNode;
    icon?: LucideIcon;
    className?: string;
}

export interface ListProps extends ListVariantProps {
    children: React.ReactNode;
    className?: string;
}

export interface ParagraphProps extends ParagraphVariantProps {
    children: React.ReactNode;
    className?: string;
}

export interface ResponsiveIconProps {
    // icon?: React.ElementType;
    icon?: LucideIcon;
    className?: string;
    onClick?: () => void;
}