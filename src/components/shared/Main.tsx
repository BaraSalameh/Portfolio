'use client';

import React from 'react';
import { cn } from '@/components/utils';
import { main } from '@/styles';
import { MainProps } from './types';

export const Main = ({
    children,
    className,
    direction,
    itemsX,
    itemsY,
    paddingX,
    paddingY,
    space
}: MainProps) => 
    <div className={cn(main({ direction, itemsX, itemsY, paddingX, paddingY, space }), className)}>
        {children}
    </div>