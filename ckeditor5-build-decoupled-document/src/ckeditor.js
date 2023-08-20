/**
 * @license Copyright (c) 2003-2021, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or https://ckeditor.com/legal/ckeditor-oss-license
 */

// The editor creator to use.
import DecoupledEditorBase from '@ckeditor/ckeditor5-editor-decoupled/src/decouplededitor';

import Essentials from '@ckeditor/ckeditor5-essentials/src/essentials';
import Alignment from '@ckeditor/ckeditor5-alignment/src/alignment';
import FontSize from '@ckeditor/ckeditor5-font/src/fontsize';
import FontFamily from '@ckeditor/ckeditor5-font/src/fontfamily';
import FontColor from '@ckeditor/ckeditor5-font/src/fontcolor';
import FontBackgroundColor from '@ckeditor/ckeditor5-font/src/fontbackgroundcolor';
import UploadAdapter from '@ckeditor/ckeditor5-adapter-ckfinder/src/uploadadapter';
import Autoformat from '@ckeditor/ckeditor5-autoformat/src/autoformat';
import Bold from '@ckeditor/ckeditor5-basic-styles/src/bold';
import Italic from '@ckeditor/ckeditor5-basic-styles/src/italic';
import Strikethrough from '@ckeditor/ckeditor5-basic-styles/src/strikethrough';
import Underline from '@ckeditor/ckeditor5-basic-styles/src/underline';
import BlockQuote from '@ckeditor/ckeditor5-block-quote/src/blockquote';
import CKFinder from '@ckeditor/ckeditor5-ckfinder/src/ckfinder';
//import EasyImage from '@ckeditor/ckeditor5-easy-image/src/easyimage';
import Heading from '@ckeditor/ckeditor5-heading/src/heading';
import Image from '@ckeditor/ckeditor5-image/src/image';
//import ImageCaption from '@ckeditor/ckeditor5-image/src/imagecaption';
//import ImageStyle from '@ckeditor/ckeditor5-image/src/imagestyle';
//import ImageToolbar from '@ckeditor/ckeditor5-image/src/imagetoolbar';
//import ImageUpload from '@ckeditor/ckeditor5-image/src/imageupload';
import Indent from '@ckeditor/ckeditor5-indent/src/indent';
import IndentBlock from '@ckeditor/ckeditor5-indent/src/indentblock';
import Link from '@ckeditor/ckeditor5-link/src/link';
import List from '@ckeditor/ckeditor5-list/src/list';
import ListStyle from '@ckeditor/ckeditor5-list/src/liststyle';
import MediaEmbed from '@ckeditor/ckeditor5-media-embed/src/mediaembed';
import Paragraph from '@ckeditor/ckeditor5-paragraph/src/paragraph';
import PasteFromOffice from '@ckeditor/ckeditor5-paste-from-office/src/pastefromoffice';
import Table from '@ckeditor/ckeditor5-table/src/table';
import TableToolbar from '@ckeditor/ckeditor5-table/src/tabletoolbar';
import TextTransformation from '@ckeditor/ckeditor5-typing/src/texttransformation';
import CloudServices from '@ckeditor/ckeditor5-cloud-services/src/cloudservices';
import SelectAll from '@ckeditor/ckeditor5-select-all/src/selectall';
import Comments from '@ckeditor/ckeditor5-comments/src/comments';



// This SVG file import will be handled by webpack's raw-text loader.
// This means that imageIcon will hold the source SVG.
import Plugin from '@ckeditor/ckeditor5-core/src/plugin';
//import imageIcon from '@ckeditor/ckeditor5-core/theme/icons/image.svg';
import imageIcon from '@ckeditor/ckeditor5-link/theme/icons/link.svg';

import ButtonView from '@ckeditor/ckeditor5-ui/src/button/buttonview';


class InsertImage extends Plugin {
	
	constructor( editor ) {
		super( editor );

	}

    init() {

		const editor = this.editor;
       
        editor.ui.componentFactory.add( 'insertImage', locale => {
            const view = new ButtonView( locale );

            view.set( {
                label: 'Insert Link',
                icon: imageIcon,
                tooltip: true
            } );
			view.bind('isEnabled').to(editor, 'isReadOnly', isReadOnly => !isReadOnly);



            // Callback executed once the image is clicked.
            view.on( 'execute', () => {

				const selection = editor.model.document.selection;
				const range = selection.getFirstRange();
				var selectedText = '';
				for (const item of range.getItems()) {
					console.log(item.data) //return the selected text
					if(item.data) {
						selectedText += " " + item.data;
					}
				}  
				
				const config = editor.config.get( 'cusMssSAett' ) || {};
				
				if (config.GetLink ) {
					config.GetLink(selectedText, ret => {

						//console.log('ret', ret);
						editor.model.change( writer => {
							const imageElement = writer.createText(selectedText, {
								linkHref: ret
							} );

							editor.model.insertContent( imageElement, editor.model.document.selection );
						} );

					});
				}
            } );

            return view;
        } );
    }
}


export default class CustomDecoupledEditor extends DecoupledEditorBase {}

// Plugins to include in the build.
CustomDecoupledEditor.builtinPlugins = [
	Essentials,
	Alignment,
	FontSize,
	FontFamily,
	FontColor,
	FontBackgroundColor,
	UploadAdapter,
	Autoformat,
	Bold,
	Italic,
	Strikethrough,
	Underline,
	BlockQuote,
	CKFinder,
	CloudServices,
	//EasyImage,
	Heading,
	Image,
	//ImageCaption,
	//ImageStyle,
	//ImageToolbar,
	//ImageUpload,
	Indent,
	IndentBlock,
	Link,
	List,
	ListStyle,
	//MediaEmbed,
	Paragraph,
	PasteFromOffice,
	Table,
	TableToolbar,
	TextTransformation,
	InsertImage,
	SelectAll,
	Comments  ,
];

// Editor configuration.
CustomDecoupledEditor.defaultConfig = {
	toolbar: {
		items: [
			'heading',
			'|',
			'fontfamily',
			'fontsize',
			'fontColor',
			'fontBackgroundColor',
			'|',
			'bold',
			'italic',
			'underline',
			'strikethrough',
			'|',
			'alignment',
			'|',
			'numberedList',
			'bulletedList',
			'|',
			'outdent',
			'indent',
			'|',
			'blockquote',
			'insertTable',
			'|',
			'undo',
			'redo',
			'|',
			'insertImage',
			'selectAll',
			'comment'
		]
	},
	image: {
		styles: [
			'full',
			'alignLeft',
			'alignRight'
		],
		toolbar: [
			'imageStyle:alignLeft',
			'imageStyle:full',
			'imageStyle:alignRight',
			'|',
			'imageTextAlternative'
		]
	},
	table: {
		contentToolbar: [
			'tableColumn',
			'tableRow',
			'mergeTableCells'
		]
	},
	/*link: {
		decorators: {
			openInNewTab: {
				mode: 'automatic',
				defaultValue: true,
				attributes: {
					target: '_blank',
					rel: 'noopener noreferrer',
					mss: 'testing'
				}
			}
		}
	},*/
	// This value must be kept in sync with the language defined in webpack.config.js.
	language: 'en'
};
