'use client';

import React, { ElementType } from 'react';
import { cn } from '@/components/utils/cn';
import { list, ListVariantProps } from '@/styles/list';

interface ListProps extends ListVariantProps {
    children: React.ReactNode;
    className?: string;
}

export const List: React.FC<ListProps> = ({
    children,
    intent,
    as,
    size,
    className,
    
}) => {
    return (
        <ol className={cn(list({ intent, size, as }), className)}>
            {children}
        </ol>
    );
};
