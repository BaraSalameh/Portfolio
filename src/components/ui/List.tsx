'use client';

import React from 'react';
import { cn } from '@/components/utils';
import { list } from '@/styles';
import { ListProps } from './types';

export const List = ({
    children,
    intent,
    as,
    size,
    className,
    
}: ListProps) => {
    return (
        <ol className={cn(list({ intent, size, as }), className)}>
            {children}
        </ol>
    );
};
