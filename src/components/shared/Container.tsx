'use client';

import React from 'react';
import { cn } from '@/components/utils';
import { container } from '@/styles';
import { ContainerProps } from './types';

export const Container = ({ children, className }: ContainerProps) => 
    <div className={cn(container(), className)}>
        {children}
    </div>