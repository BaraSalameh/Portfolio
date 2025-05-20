import React from 'react';

interface WithSkeletonProps {
  isLoading: boolean;
  skeleton: React.ReactNode;
  children: React.ReactNode;
}

const WithSkeleton: React.FC<WithSkeletonProps> = ({ isLoading, skeleton, children }) => {
  return <React.Fragment>{isLoading ? skeleton : children}</React.Fragment>;
};

export default WithSkeleton;
