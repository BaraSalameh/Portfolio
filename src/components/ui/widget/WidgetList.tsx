import React, { useEffect, useState } from 'react';
import { Paragraph } from '../Paragraph';
import ResponsiveIcon from '../ResponsiveIcon';
import dayjs from 'dayjs';
import { widgetList } from '@/styles/widget';
import { cn } from '@/components/utils/cn';
import { closestCenter, DndContext, PointerSensor, useSensor, useSensors } from '@dnd-kit/core';
import { arrayMove, SortableContext, verticalListSortingStrategy } from '@dnd-kit/sortable';
import { SortableItem } from './SortableItem';
import { WidgetListProps } from './type';
import { extractValue } from '@/lib/utils/appFunctions';

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
                        const leftRaw = cfg.leftKey ? extractValue(item, cfg.leftKey) : undefined;
                        const rightRaw = cfg.rightKey ? extractValue(item, cfg.rightKey) : undefined;

                        const leftVal = cfg.isTime
                            ? dayjs(leftRaw).format('MMM YYYY')
                            : leftRaw;

                        const rightVal = cfg.isTime
                            ?   rightRaw
                                    ? dayjs(rightRaw).format('MMM YYYY')
                                    : 'Present'
                            : cfg.rightKey
                                ?   rightRaw
                                :   '';

                        return (
                            <Paragraph key={index} size={cfg.size}>
                                {cfg.icon && <ResponsiveIcon icon={cfg.icon} />}

                                {Array.isArray(leftVal)
                                    ?   leftVal.length > 0
                                            ?   leftVal.map((val, idx) => (
                                                    <code key={idx}>
                                                        {val}
                                                        {idx !== leftVal.length - 1 && ' | '}
                                                    </code>
                                                ))
                                            :   'Empty'  
                                        :   typeof leftVal === 'boolean'
                                                ? leftVal
                                                    ? `${cfg.leftKey}: ${leftVal}`
                                                    : ''
                                                : leftVal
                                }
                                {cfg.between && ` ${cfg.between} `}

                                {rightVal}
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
                        <div key={idx}>{listItem}</div>
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
