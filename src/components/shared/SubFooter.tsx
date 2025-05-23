'use client';

import React from 'react';
import { cn } from '@/components/utils';
import { subFooter } from '@/styles';
import { SubFooterProps } from './types';

export const SubFooter = ({
    children,
    className,
    itemsX,
    itemsY,
    paddingX,
    paddingY,
    space
}: SubFooterProps) => 
    <div className={cn(subFooter({ itemsX, itemsY, paddingX, paddingY, space}), className)}>
        {children}
    </div>
