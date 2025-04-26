'use client';

import React from 'react';
import { cn } from '@/components/utils/cn';
import { container, ContainerVariantProps } from '@/styles/container';

interface ContainerProps extends ContainerVariantProps {
    children: React.ReactNode;
    className?: string;
}

export const Container: React.FC<ContainerProps> = ({
    children,
    className,
}) => {
    return (
        <div className={cn(container(), className)}>
            {children}
        </div>
    );
};
