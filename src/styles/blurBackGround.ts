import { cva, VariantProps } from 'class-variance-authority';

export const blurBackGround = cva(
    'fixed inset-0 flex items-center justify-center z-50',
    {
        variants: {
            intent: {
                sm: 'bg-black/25 backdrop-blur-sm bg-opacity-25',
                md: 'bg-black/50 backdrop-blur-md bg-opacity-50',
                lg: 'bg-black/75 backdrop-blur-lg bg-opacity-75'
            }
        },
        defaultVariants: {
            intent: 'md'
        },
    }
);

export type BlurBackGroundVariantProps = VariantProps<typeof blurBackGround>;
