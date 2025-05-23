'use client';

import React from "react";
import { BlurBackground, Paragraph } from "../ui";
import { LoadingProps } from "./types";

export const Loading = ({ message = "Loading...", fullScreen = true, isLoading = false }: LoadingProps) => {
    return (
        isLoading
        ?   <BlurBackground fullScreen={fullScreen}>
                <div className="flex flex-col items-center space-y-4" >
                    <div className="w-12 h-12 border-4 border-green-900 border-dashed rounded-full animate-[spin_5s_linear_infinite]" />
                    <Paragraph size='lg'>{message}</Paragraph>
                </div>
            </BlurBackground>
        :   null
    );
};