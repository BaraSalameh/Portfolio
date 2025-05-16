import React from "react";
import { BlurBackGround } from "../ui/BlurBackGround";
import { Paragraph } from "../ui/Paragraph";

interface LoadingProps {
    message?: string;
    fullScreen?: boolean;
    isLoading?: boolean;
    className?: string;
}

const Loading: React.FC<LoadingProps> = ({ message = "Loading...", fullScreen = true, isLoading = false }) => {
    return (
        isLoading
        ?   <BlurBackGround fullScreen={fullScreen}>
                <div className="flex flex-col items-center space-y-4" >
                    <div className="w-12 h-12 border-4 border-green-900 border-dashed rounded-full animate-[spin_5s_linear_infinite]" />
                    <Paragraph size='lg'>{message}</Paragraph>
                </div>
            </BlurBackGround>
        :   null
    );
};

export default Loading;
