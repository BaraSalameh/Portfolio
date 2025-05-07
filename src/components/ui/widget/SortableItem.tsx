import React from 'react';
import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';

interface SortableItemProps {
    id: string;
    child: React.ReactNode;
}

export const SortableItem: React.FC<SortableItemProps> = ({ id, child }) => {
    const {
        attributes,
        listeners,
        setNodeRef,
        setActivatorNodeRef,
        transform,
        transition,
    } = useSortable({ id });

    const style = {
        transform: CSS.Transform.toString(transform),
        transition,
    };

    return (
        <div ref={setNodeRef} style={style}>
            <div
                ref={setActivatorNodeRef}
                {...attributes}
                {...listeners}
                style={{ cursor: 'grab' }}
            >
                {child}
            </div>
        </div>
    );
};
