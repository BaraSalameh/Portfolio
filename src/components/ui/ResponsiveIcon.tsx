import React from 'react';
import { PlusIcon } from 'lucide-react'; 
import { ResponsiveIconProps } from './types';

export const ResponsiveIcon = ({ icon: Icon = PlusIcon, className, onClick }: ResponsiveIconProps) => 
    <Icon className={`w-4.5 h-4.5 ${className}`} onClick={onClick} />;