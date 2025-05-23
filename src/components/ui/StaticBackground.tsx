import React from "react";
import { Main } from "../shared";

export const StaticBackground = () => {
    return (
        <React.Fragment>
            <div className='px-7 sm:px-10 lg:px-15 pt-3 space-y-1 w-full'>
                <div className="animate-pulse bg-gray-600 w-[15%] h-6 rounded" />
                <div className="animate-pulse bg-gray-600 w-[25%] h-5 rounded" />
                <div className="animate-pulse bg-gray-600 w-[10%] h-4 rounded" />
                <hr className="mb-3"/>
                <div className="animate-pulse bg-gray-600 w-full h-4 rounded" />
                <div className="animate-pulse bg-gray-600 w-[50%] h-4 rounded" />
                <br />
                <div className="animate-pulse bg-gray-600 w-full h-4 rounded" />
                <div className="animate-pulse bg-gray-600 w-[50%] h-4 rounded" />
                <br />
                <div className="animate-pulse bg-gray-600 w-full h-4 rounded" />
                <div className="animate-pulse bg-gray-600 w-[50%] h-4 rounded" />
                <br />
            </div>
        <Main>
            <div className="columns-1 sm:columns-2 lg:columns-3 gap-4 space-y-3 w-full">
                <div className="break-inside-avoid animate-pulse bg-green-900 w-full h-90 rounded" />
                <div className="break-inside-avoid animate-pulse bg-green-900 w-full h-90 rounded" />
                <div className="break-inside-avoid animate-pulse bg-green-900 w-full h-90 rounded" />
                <div className="break-inside-avoid animate-pulse bg-green-900 w-full h-90 rounded" />
                <div className="break-inside-avoid animate-pulse bg-green-900 w-full h-90 rounded" />
                <div className="break-inside-avoid animate-pulse bg-green-900 w-full h-90 rounded" />
                <div className="break-inside-avoid animate-pulse bg-green-900 w-full h-90 rounded" />
                <div className="break-inside-avoid animate-pulse bg-green-900 w-full h-90 rounded" />
                <div className="break-inside-avoid animate-pulse bg-green-900 w-full h-90 rounded" />
            </div>
        </Main>
        </React.Fragment>
    );
};