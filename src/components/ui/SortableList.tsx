'use client';

import { useState } from 'react';
import {
    DndContext,
    closestCenter,
    PointerSensor,
    useSensor,
    useSensors,
} from '@dnd-kit/core';
import {
    arrayMove,
    SortableContext,
    useSortable,
    verticalListSortingStrategy,
} from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import { dynamicApi } from '@/lib/apis/apiClient';
import { useAppDispatch } from '@/lib/store/hooks';
import { educationListQuery } from '@/lib/apis/owner/educationListQuery';

interface SortableEducationListProps {
    initialItems: Record<string, any>[];
}

export default function SortableEducationList({
    initialItems,
}: SortableEducationListProps) {
    const [items, setItems] = useState<Record<string, any>[]>(initialItems);
    const dispatch = useAppDispatch();
    const sensors = useSensors(useSensor(PointerSensor));

    const handleDragEnd = async (event: any) => {
        const { active, over } = event;

        if (active.id !== over.id) {
            const oldIndex = items.findIndex((item) => item.id === active.id);
            const newIndex = items.findIndex((item) => item.id === over.id);

            const newItems = arrayMove(items, oldIndex, newIndex);
            setItems(newItems);

            const orderedIds = newItems.map((item) => item.id);

            console.log(orderedIds);

            try {
                await dynamicApi({
                    method: 'POST',
                    url: '/Owner/ReOrderEducation',
                    data: {educationIdsInOrder: orderedIds},
                    withCredentials: true
                });
                await dispatch(educationListQuery());
            } catch (err) {
                console.error('Failed to update order', err);
            }
        }
    };

    return (
        <div className='max-h-[40vh] overflow-auto scrollbar-hide'>
        <DndContext
            sensors={sensors}
            collisionDetection={closestCenter}
            onDragEnd={handleDragEnd}
        >
            <SortableContext 
                items={items.map((i) => i.id)} 
                strategy={verticalListSortingStrategy}
            >
                <div className="space-y-2">
                    {items.map((item) => (
                        <SortableItem 
                            key={item.id} 
                            id={item.id} 
                            label={`${item.degree} – ${item.institution}`} 
                        />
                    ))}
                </div>
                </SortableContext>
        </DndContext>
        </div>
    );
}

interface SortableItemProps {
    id: string;
    label: string;
}

function SortableItem({ id, label }: SortableItemProps) {
    const { attributes, listeners, setNodeRef, transform, transition } = useSortable({ id });

    const style = {
        transform: CSS.Transform.toString(transform),
        transition,
        padding: 12,
        border: '1px solid #ccc',
        borderRadius: 6,
        background: 'green',
        cursor: 'grab',
    };

    return (
        <div ref={setNodeRef} style={style} {...attributes} {...listeners}>
            {label}
        </div>
    );
}
