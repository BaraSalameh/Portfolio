'use client';

import React from 'react';
import { cn } from '@/components/utils';
import { header } from '@/styles';
import { HeaderProps } from './types';

export const Header = ({
    children,
    className,
    itemsX,
    itemsY,
    paddingX,
    paddingY,
    space
}: HeaderProps) => 
    <div className={cn(header({ itemsX, itemsY, paddingX, paddingY, space}), className)}>
        {children}
    </div>