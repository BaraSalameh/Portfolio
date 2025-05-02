'use client'

import { Container } from "@/components/shared/Container";
import Sidebar from "@/components/shared/Sidebar";
import { useAppSelector } from "@/lib/store/hooks";
import { useRouter } from "next/navigation";
import { useEffect } from "react";

export default function ClientLayout({children}: Readonly<{children: React.ReactNode;}>) {

    const router = useRouter();
    const { error } = useAppSelector(state => state.user);

    useEffect(() => {
        if(error) {
            router.push('/');
        }
    }, [error]);

    return (
        <div className='flex'>
            <Sidebar />
            <Container>
                {children}
            </Container>
        </div>
    );
};