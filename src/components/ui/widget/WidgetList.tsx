import React, { useEffect, useState } from 'react';
import { Paragraph } from '../Paragraph';
import ResponsiveIcon from '../ResponsiveIcon';
import dayjs from 'dayjs';
import { widgetList, WidgetListVariantProps } from '@/styles/widget';
import { cn } from '@/components/utils/cn';
import { closestCenter, DndContext, PointerSensor, useSensor, useSensors } from '@dnd-kit/core';
import { arrayMove, SortableContext, verticalListSortingStrategy } from '@dnd-kit/sortable';
import { SortableItem } from './SortableItem';

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
    onItemClick?: (item: any) => void;
    className?: string;
    sort?: {
        sortable: boolean;
        onSort?: (lstIds: string[]) => any;
    }
}

export const WidgetList: React.FC<WidgetListProps> = ({ items, list, onItemClick, className, sort }) => {

    const [rows, setRows] = useState<Record<string, any>[]>(items);
    const sensors = useSensors(useSensor(PointerSensor));

    const handleDragEnd = async (event: any) => {
        const { active, over } = event;

        if (!over || active.id === over.id) return;

        const oldIndex = rows.findIndex((item) => item.id === active.id);
        const newIndex = rows.findIndex((item) => item.id === over.id);

        const newItems = arrayMove(rows, oldIndex, newIndex);
        setRows(newItems);

        const orderedIds = newItems.map((item) => item.id);

        sort?.onSort?.(orderedIds);
    };

    useEffect(() => {
        setRows(items);
    }, [items]);

    const renderList = () =>
        rows.map((item, idx) => {
            const listItem = (
                <li
                    key={item.id ?? idx}
                    className={`${cn(widgetList({ clickable: sort?.sortable ? false : onItemClick ? true : false }), className)}`}
                    onClick={() => onItemClick?.(item)}
                >
                    {list.map((cfg, index) => {
                        const leftVal = cfg.isTime
                            ? dayjs(item[cfg.leftKey]).format('MMM YYYY')
                            : item[cfg.leftKey];
                        const rightVal = cfg.isTime
                            ? cfg.rightKey && item[cfg.rightKey]
                                ? dayjs(item[cfg.rightKey]).format('MMM YYYY')
                                : 'Present'
                            : cfg.rightKey
                                ? item[cfg.rightKey]
                                : '';

                        return (
                            <Paragraph key={index} size={cfg.size}>
                                {cfg.icon && <ResponsiveIcon icon={cfg.icon} />}
                                {leftVal} {cfg.between ?? ''} {rightVal}
                            </Paragraph>
                        );
                    })}
                </li>
            );

            return sort?.sortable
                ?   (
                        <SortableItem key={item.id} id={item.id} child={listItem} />
                    )
                :   (
                        <div key={item.id}>{listItem}</div>
                    );
        });

        return sort?.sortable ? (
            <DndContext
                sensors={sensors}
                collisionDetection={closestCenter}
                onDragEnd={handleDragEnd}
            >
                <SortableContext
                    items={rows.map((i) => i.id)}
                    strategy={verticalListSortingStrategy}
                >
                    {renderList()}
                </SortableContext>
            </DndContext>
        ) : (
            <React.Fragment>{renderList()}</React.Fragment>
        );
};
