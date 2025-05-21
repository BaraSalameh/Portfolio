'use client'

import { Container } from "@/components/shared/Container";
import Sidebar from "@/components/shared/Sidebar";
import { useAppSelector } from "@/lib/store/hooks";
import { useParams, useRouter } from "next/navigation";
import { useEffect } from "react";

export default function OwnerLayout({children}: Readonly<{children: React.ReactNode;}>) {

    const router = useRouter();
    const { username: username, role } = useParams<{ username: string; role: 'owner' | 'client' | 'admin' }>();
    const { user: owner } = useAppSelector(state => state.owner);
    
    useEffect(() => {
        if(role === 'owner' && owner?.username && owner.username !== username){
            router.replace(`/owner/${owner.username}/dashboard`);
        }
    }, [owner?.username]);

    return (
        <div className='flex'>
            <Sidebar />
            <Container>
                {children}
            </Container>
        </div>
    );
};