'use client';

import Sidebar from "@/components/shared/Sidebar";
import { userByUsernameQuery } from "@/lib/apis/client/userBuUsernameQuery";
import { useAppDispatch } from "@/lib/store/hooks";
import { useParams } from "next/navigation";
import { useEffect } from "react";

export default function ClientHomePage() {

    const dispatch = useAppDispatch();
    const { username } = useParams<{username: string}>();
    
    useEffect(() => {
        username && dispatch(userByUsernameQuery(username));
    }, [username]);

    return (
        <div className="flex items-center justify-center">
            <Sidebar />
            <h1>Hello I am a client</h1>
        </div>
    );
}
