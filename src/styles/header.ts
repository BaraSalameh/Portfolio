import { cva, VariantProps } from 'class-variance-authority';

export const header = cva(
    'flex justify-start items-center p-4 w-full row-start-1'
);

export type HeaderVariantProps = VariantProps<typeof header>;
