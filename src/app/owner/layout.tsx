'use client'

import Sidebar from "@/components/shared/Sidebar";
import { useAppSelector } from "@/lib/store/hooks";
import { useCheckAndGetUsername } from "@/lib/utils/appFunctions";
import { useRouter } from "next/navigation";
import { useEffect } from "react";

export default function OwnerLayout({children}: Readonly<{children: React.ReactNode;}>) {

    const router = useRouter();
    const getUsername = useCheckAndGetUsername();
    const username = useAppSelector(state => state.auth.username);
    const role = useAppSelector(state => state.auth.role);

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
        <>
            <Sidebar role={role} />
            {children}
        </>
    );
};