import React from 'react';
import { Paragraph } from '../Paragraph';
import ResponsiveIcon from '../ResponsiveIcon';
import dayjs from 'dayjs';
import { widgetList, WidgetListVariantProps } from '@/styles/widget';
import { cn } from '@/components/utils/cn';

export interface ListItemConfig {
    icon?: any;
    leftKey: string;
    between?: string;
    rightKey?: string;
    size?: 'lg' | 'md' | 'sm' | null;
    isTime?: boolean;
}

export interface WidgetListProps extends WidgetListVariantProps {
    items: Record<string, any>[];
    list: ListItemConfig[];
    onItemClick?: (id: string) => void;
    className?: string;
}

export const WidgetList: React.FC<WidgetListProps> = ({ items, list, onItemClick, className }) => {
    return (
        <React.Fragment>
            {items.map((item, idx) => (
                <li key={item.id ?? idx} className={cn(widgetList({}), className)} onClick={onItemClick ? () => onItemClick(item.id) : undefined}>
                    {list.map((cfg, index) => {
                        const leftVal = cfg.isTime ? dayjs(item[cfg.leftKey]).format('MMM YYYY') : item[cfg.leftKey];
                        const rightVal = cfg.isTime
                        ? cfg.rightKey && item[cfg.rightKey]
                            ? dayjs(item[cfg.rightKey]).format('MMM YYYY')
                            : 'Present'
                        : cfg.rightKey
                            ? item[cfg.rightKey]
                            : '';

                        return (
                            <Paragraph key={index} size={cfg.size} className='gap-3'>
                                {cfg.icon && <ResponsiveIcon icon={cfg.icon} />}
                                {leftVal} {cfg.between ?? ''} {rightVal}
                            </Paragraph>
                        );
                    })}
                </li>
            ))}
        </React.Fragment>
    );
};
