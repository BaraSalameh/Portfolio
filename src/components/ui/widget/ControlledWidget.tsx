import React from "react";
import { WidgetCardProps } from "./type";
import { useParams } from "next/navigation";
import { WidgetCard } from "./WidgetCard";

const ControlledWidget = React.memo((props: WidgetCardProps) => {
    const { role } = useParams<{ role: 'owner' | 'client' | 'admin', username: string }>();
    const isOwner = role === 'owner';

    return (
        <WidgetCard
            {...props}
            create={isOwner ? props.create : undefined}
            update={isOwner ? props.update : undefined}
            del={isOwner ? props.del : undefined}
            onSort={isOwner ? props.onSort : undefined}
        />
    );
});

export default ControlledWidget;