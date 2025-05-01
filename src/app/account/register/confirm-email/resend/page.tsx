'use client';

import { useEffect } from "react";
import { useRouter, useSearchParams } from "next/navigation";
import { useAppDispatch } from "@/lib/store/hooks";
import { resendEmail } from "@/lib/apis/account/resendEmail";

export default function ResendPage() {

    const searchParams = useSearchParams();
    const email = searchParams.get('email');
    var router = useRouter();
    var dispatch = useAppDispatch();
    
    useEffect(() => {
        if (!email) return;
    
        dispatch(resendEmail({
            email,
        }));
    
        router.push(`/account/register/confirm-email`);
    }, [email]);
}
