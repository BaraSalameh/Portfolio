'use client'

import { Container } from "@/components/shared/Container";
import { Main } from "@/components/shared/Main";
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
        <div className='flex'>
            <Sidebar role={role} />
            <Container>
                {children}
            </Container>
        </div>
    );
};