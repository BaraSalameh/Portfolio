import React from 'react';
import { WithSkeletonProps } from './types';

export const WithSkeleton = ({ isLoading, skeleton, children }: WithSkeletonProps) => 
    <React.Fragment>
        {isLoading ? skeleton : children}
    </React.Fragment>
