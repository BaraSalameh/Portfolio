import { cva, VariantProps } from 'class-variance-authority';

export const container = cva(
    'relative grid grid-rows-[100px_1fr_100px] items-center justify-items-center sm:px-20 p-5 font-[family-name:var(--font-geist-sans)]'
);

export type ContainerVariantProps = VariantProps<typeof container>;
