'use client'

import { Container } from "@/components/shared/Container";
import { Header } from "@/components/shared/Header";
import Sidebar from "@/components/shared/Sidebar";
import { Paragraph } from "@/components/ui/Paragraph";
import { useAppSelector } from "@/lib/store/hooks";
import { useCheckAndGetUsername } from "@/lib/utils/appFunctions";
import { useRouter } from "next/navigation";
import { useEffect } from "react";

export default function OwnerLayout({children}: Readonly<{children: React.ReactNode;}>) {

    const router = useRouter();
    const getUsername = useCheckAndGetUsername();
    const { username, role, loading } = useAppSelector(state => state.auth);

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