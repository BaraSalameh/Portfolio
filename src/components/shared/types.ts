import { ContainerVariantProps, SubFooterVariantProps, MainVariantProps, HeaderVariantProps } from "@/styles";

interface FCProps {
    children: React.ReactNode;
    className?: string;
}

export interface ContainerProps extends ContainerVariantProps, FCProps {}

export interface HeaderProps extends HeaderVariantProps, FCProps {}

export interface LoadingProps {
    message?: string;
    fullScreen?: boolean;
    isLoading?: boolean;
    className?: string;
}

export interface MainProps extends MainVariantProps, FCProps {}

export interface SubFooterProps extends SubFooterVariantProps, FCProps {}

export interface WithSkeletonProps {
    isLoading: boolean;
    skeleton: React.ReactNode;
    children: React.ReactNode;
}