'use client';

import Sidebar from "@/components/shared/Sidebar";
import { useAppSelector } from "@/lib/store/hooks";
import { useCheckAndGetUsername } from "@/lib/utils/appFunctions";
import { useRouter } from "next/navigation";
import { useEffect } from "react";

export default function OwnerHomePage() {

    const router = useRouter();
    const getUsername = useCheckAndGetUsername();
    const username = useAppSelector(state => state.auth.username);

    useEffect(() => {
        const run = async () => {
            const u = username || await getUsername();
            if (!u) {
                router.push(`/`);
            }
        };
        run();
    }, [username]);

    return (
        <div className="flex items-center justify-center">
            <Sidebar role="owner" />
            <h1>Hello I am an owner!</h1>
        </div>
    );
}
