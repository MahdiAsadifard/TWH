
import { Spinner, SpinnerProps } from "@fluentui/react-components";
import React from "react";

interface IProps {
    SnipperProps?: Partial<SpinnerProps>;
    FullScreen?: boolean;
};

const Loading: React.FC<IProps> = ({
    SnipperProps,
    FullScreen = false,
}): React.ReactElement => {
    const full_page = {
        display: 'block',
        height: '100vh',
        alignContent: 'center'
    };
    return (
        <span style={FullScreen ? full_page : {}}>
            <Spinner labelPosition="below" label='Loading...' {...SnipperProps} />
        </span>
    );
};

export default Loading;