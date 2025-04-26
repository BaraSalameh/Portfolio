'use client';

import { Card } from "@/components/ui/Card";
import { useAppSelector } from "@/lib/store/hooks";
import { useRouter } from "next/navigation";

export default function OwnerHomePage() {

    const router = useRouter();
    const user = useAppSelector(state => state.user);

    return (
        <div className="flex items-center justify-center">
            <Card title="Arab american university" description="Computer science" label="Learn more" />
        </div>
    );
}
