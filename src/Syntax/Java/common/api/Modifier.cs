using System;
using System.Collections.Generic;
using System.Text;

namespace java.lang.common.api
{
    public enum Modifier
    {
        // See JLS sections 8.1.1, 8.3.1, 8.4.3, 8.8.3, and 9.1.1.
        // java.lang.reflect.Modifier includes INTERFACE, but that's a VMism.

        /** The modifier {@code public} */
        PUBLIC,
        /** The modifier {@code protected} */
        PROTECTED,
        /** The modifier {@code private} */
        PRIVATE,
        /** The modifier {@code abstract} */
        ABSTRACT,
        /**
         * The modifier {@code default}
         * @since 1.8
         */
        DEFAULT,
        /** The modifier {@code static} */
        STATIC,
        /// <summary>
        /// {@preview Associated with sealed classes, a preview feature of the Java language.
        /// 
        ///           This enum constant is associated with <i>sealed classes</i>, a preview
        ///           feature of the Java language. Preview features
        ///           may be removed in a future release, or upgraded to permanent
        ///           features of the Java language.}
        /// 
        /// The modifier {@code sealed}
        /// @since 15
        /// </summary>
        SEALED,
        /// <summary>
        /// {@preview Associated with sealed classes, a preview feature of the Java language.
        /// 
        ///           This enum constant is associated with <i>sealed classes</i>, a preview
        ///           feature of the Java language. Preview features
        ///           may be removed in a future release, or upgraded to permanent
        ///           features of the Java language.}
        /// 
        /// The modifier {@code non-sealed}
        /// @since 15
        /// </summary>
        NON_SEALED,

        /** The modifier {@code final} */
        FINAL,
        /** The modifier {@code transient} */
        TRANSIENT,
        /** The modifier {@code volatile} */
        VOLATILE,
        /** The modifier {@code synchronized} */
        SYNCHRONIZED,
        /** The modifier {@code native} */
        NATIVE,
        /** The modifier {@code strictfp} */
        STRICTFP
    }
}
