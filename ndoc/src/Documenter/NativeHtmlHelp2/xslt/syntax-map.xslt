<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template match="@* | node() | text()" mode="access"/>
	
	<xsl:template match="field | method | constructor | event | property | operator" mode="access">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">
				<xsl:choose>
					<xsl:when test="@access='Public'">Public</xsl:when>
					<xsl:when test="@access='Family'">Protected</xsl:when>
					<xsl:when test="@access='FamilyOrAssembly'">Protected Friend</xsl:when>
					<xsl:when test="@access='Assembly'">Friend</xsl:when>
					<xsl:when test="@access='Private'">Private</xsl:when>
					<xsl:otherwise>/* unknown */</xsl:otherwise>			
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$lang='C#'">
				<xsl:choose>
					<xsl:when test="@access='Public'">public</xsl:when>
					<xsl:when test="@access='Family'">protected</xsl:when>
					<xsl:when test="@access='FamilyOrAssembly'">protected internal</xsl:when>
					<xsl:when test="@access='Assembly'">internal</xsl:when>
					<xsl:when test="@access='Private'">private</xsl:when>
					<xsl:otherwise>/* unknown */</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$lang='C++'">
				<xsl:choose>
					<xsl:when test="@access='Public'">public:</xsl:when>
					<xsl:when test="@access='Family'">protected:</xsl:when>
					<xsl:when test="@access='FamilyOrAssembly'">public protected:</xsl:when>
					<xsl:when test="@access='Assembly'">public private:</xsl:when>
					<xsl:when test="@access='Private'">private:</xsl:when>
					<xsl:otherwise>/* unknown */</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$lang='JScript'">
				<xsl:choose>
					<xsl:when test="@access='Public'">public</xsl:when>
					<xsl:when test="@access='Family'">protected</xsl:when>
					<xsl:when test="@access='FamilyOrAssembly'">protected internal</xsl:when>
					<xsl:when test="@access='Assembly'">internal</xsl:when>
					<xsl:when test="@access='Private'">private</xsl:when>
					<xsl:otherwise>/* unknown */</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
		</xsl:choose>
		<xsl:text>&#160;</xsl:text>
	</xsl:template>
	<!-- -->
	<xsl:template match="structure | interface | class | delegate | enumeration" mode="access">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">
				<xsl:choose>
					<xsl:when test="@access='Public'">Public</xsl:when>
					<xsl:when test="@access='NotPublic'">Friend</xsl:when>
					<xsl:when test="@access='NestedPublic'">Public</xsl:when>
					<xsl:when test="@access='NestedFamily'">Protected</xsl:when>
					<xsl:when test="@access='NestedFamilyOrAssembly'">Protected Friend</xsl:when>
					<xsl:when test="@access='NestedAssembly'">Friend</xsl:when>
					<xsl:when test="@access='NestedPrivate'">Private</xsl:when>
					<xsl:otherwise>/* unknown */</xsl:otherwise>			
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$lang='C#'">
				<xsl:choose>
					<xsl:when test="@access='Public'">public</xsl:when>
					<xsl:when test="@access='NotPublic'">internal</xsl:when>
					<xsl:when test="@access='NestedPublic'">public</xsl:when>
					<xsl:when test="@access='NestedFamily'">protected</xsl:when>
					<xsl:when test="@access='NestedFamilyOrAssembly'">protected internal</xsl:when>
					<xsl:when test="@access='NestedAssembly'">internal</xsl:when>
					<xsl:when test="@access='NestedPrivate'">private</xsl:when>
					<xsl:otherwise>/* unknown */</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$lang='C++'">
				<xsl:choose>
					<xsl:when test="@access='Public'">public</xsl:when>
					<xsl:when test="@access='NotPublic'">private</xsl:when>
					<xsl:when test="@access='NestedPublic'">public</xsl:when>
					<xsl:when test="@access='NestedFamily'">protected</xsl:when>
					<xsl:when test="@access='NestedFamilyOrAssembly'">public private</xsl:when>
					<xsl:when test="@access='NestedAssembly'">protected private</xsl:when>
					<xsl:when test="@access='NestedPrivate'">private</xsl:when>
					<xsl:otherwise>/* unknown */</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$lang='JScript'">
				<xsl:if test="local-name() != 'structure' and local-name() != 'delegate'">
					<xsl:choose>
						<xsl:when test="@access='Public'">public</xsl:when>
						<xsl:when test="@access='NotPublic'">internal</xsl:when>
						<xsl:when test="@access='NestedPublic'">public</xsl:when>
						<xsl:when test="@access='NestedFamily'">protected</xsl:when>
						<xsl:when test="@access='NestedFamilyOrAssembly'">protected internal</xsl:when>
						<xsl:when test="@access='NestedAssembly'">internal</xsl:when>
						<xsl:when test="@access='NestedPrivate'">private</xsl:when>
						<xsl:otherwise>/* unknown */</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:when>
		</xsl:choose>
		<xsl:text>&#160;</xsl:text>
	</xsl:template>
	
	<xsl:template match="node()" mode="contract">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">
				<xsl:choose>
					<xsl:when test="@contract='Static'">Shared</xsl:when>
					<xsl:when test="@contract='Abstract'">MustOverride</xsl:when>
					<xsl:when test="@contract='Final'"></xsl:when>
					<xsl:when test="@contract='Virtual'">Overridable</xsl:when>
					<xsl:when test="@contract='Override'">Overrides</xsl:when>
					<xsl:when test="@contract='Normal'"></xsl:when>
					<xsl:otherwise>/* unknown */</xsl:otherwise>
				</xsl:choose>			
			</xsl:when>		
			<xsl:when test="$lang='C#'">
				<xsl:choose>
					<xsl:when test="@contract='Static'">static</xsl:when>
					<xsl:when test="@contract='Abstract'">abstract</xsl:when>
					<xsl:when test="@contract='Final'"></xsl:when>
					<xsl:when test="@contract='Virtual'">virtual</xsl:when>
					<xsl:when test="@contract='Override'">override</xsl:when>
					<xsl:when test="@contract='Normal'"></xsl:when>
					<xsl:otherwise>/* unknown */</xsl:otherwise>
				</xsl:choose>			
			</xsl:when>
			<xsl:when test="$lang='C++'">
				<xsl:choose>
					<xsl:when test="@contract='Static'">static</xsl:when>
					<xsl:when test="@contract='Abstract'">abstract</xsl:when>
					<xsl:when test="@contract='Final'"></xsl:when>
					<xsl:when test="@contract='Virtual'">virtual</xsl:when>
					<xsl:when test="@contract='Override'"></xsl:when>
					<xsl:when test="@contract='Normal'"></xsl:when>
					<xsl:otherwise>/* unknown */</xsl:otherwise>
				</xsl:choose>			
			</xsl:when>
			<xsl:when test="$lang='JScript'">
				<xsl:choose>
					<xsl:when test="@contract='Static'">static</xsl:when>
					<xsl:when test="@contract='Abstract'">abstract</xsl:when>
					<xsl:when test="@contract='Final'"></xsl:when>
					<xsl:when test="@contract='Virtual'">virtual</xsl:when>
					<xsl:when test="@contract='Override'">override</xsl:when>
					<xsl:when test="@contract='Normal'"></xsl:when>
					<xsl:otherwise>/* unknown */</xsl:otherwise>
				</xsl:choose>			
			</xsl:when>
		</xsl:choose>
		<xsl:text>&#160;</xsl:text>
	</xsl:template>

	<xsl:template name="constructor-keyword">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">Sub&#160;New&#160;</xsl:when>
			<xsl:when test="$lang='JScript'">function&#160;</xsl:when>
		</xsl:choose>		
	</xsl:template>
	
	<xsl:template name="statement-end">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='C#'">;</xsl:when>
			<xsl:when test="$lang='C++'">;</xsl:when>
			<xsl:when test="$lang='JScript'">;</xsl:when>
		</xsl:choose>		
	</xsl:template>	

	<xsl:template name="statement-continue">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">&#160;_&#160;</xsl:when>
		</xsl:choose>		
	</xsl:template>	
<!--
	<xsl:template name="">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'"></xsl:when>
			<xsl:when test="$lang='C#'">;</xsl:when>
			<xsl:when test="$lang='C++'">;</xsl:when>
			<xsl:when test="$lang='JScript'">;</xsl:when>
		</xsl:choose>	
	</xsl:template>
		-->	


	<xsl:template match="@* | node() | text()" mode="inherits"/>
	<xsl:template match="structure | interface | class" mode="inherits">
		<xsl:param name="lang"/>
		<xsl:text>&#160;</xsl:text>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">
<xsl:text>
   </xsl:text>Inherits</xsl:when>
			<xsl:when test="$lang='C#'">:</xsl:when>
			<xsl:when test="$lang='C++'">: public</xsl:when>
			<xsl:when test="$lang='JScript'">extends</xsl:when>
		</xsl:choose>	
		<xsl:text>&#160;</xsl:text>
	</xsl:template>

	<xsl:template match="@* | node() | text()" mode="abstract"/>
	<xsl:template match="class[@abstract='true']" mode="abstract">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">MustInherit</xsl:when>
			<xsl:when test="$lang='C#'">abstract</xsl:when>
			<xsl:when test="$lang='C++'">__abstract</xsl:when>
		</xsl:choose>	
		<xsl:text>&#160;</xsl:text>
	</xsl:template>

	
	<xsl:template match="@* | node() | text()" mode="sealed"/>
	<xsl:template match="class[@sealed='true']" mode="sealed">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">NotInheritable</xsl:when>
			<xsl:when test="$lang='C#'">sealed</xsl:when>
			<xsl:when test="$lang='C++'">__sealed</xsl:when>
		</xsl:choose>	
		<xsl:text>&#160;</xsl:text>
	</xsl:template>

	<xsl:template match="@* | node() | text()" mode="keyword"/>
	<xsl:template match="class" mode="keyword">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">Class</xsl:when>
			<xsl:when test="$lang='C#'">class</xsl:when>
			<xsl:when test="$lang='C++'">class</xsl:when>
			<xsl:when test="$lang='JScript'">class</xsl:when>
		</xsl:choose>	
		<xsl:text>&#160;</xsl:text>
	</xsl:template>
	<xsl:template match="structure" mode="keyword">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">Structure</xsl:when>
			<xsl:when test="$lang='C#'">struct</xsl:when>
			<xsl:when test="$lang='C++'">struct</xsl:when>
			<xsl:when test="$lang='JScript'">In JScript, you can use the structures in the .NET Framework, but you cannot define your own.</xsl:when>
		</xsl:choose>	
		<xsl:text>&#160;</xsl:text>
	</xsl:template>
	<xsl:template match="interface" mode="keyword">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">Interface</xsl:when>
			<xsl:when test="$lang='C#'">interface</xsl:when>
			<xsl:when test="$lang='C++'">__interface</xsl:when>
			<xsl:when test="$lang='JScript'">interface</xsl:when>
		</xsl:choose>	
		<xsl:text>&#160;</xsl:text>
	</xsl:template>
	<xsl:template match="enumeration" mode="keyword">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">Enum</xsl:when>
			<xsl:when test="$lang='C#'">enum</xsl:when>
			<xsl:when test="$lang='C++'">enum</xsl:when>
			<xsl:when test="$lang='JScript'">enum</xsl:when>
		</xsl:choose>	
		<xsl:text>&#160;</xsl:text>
	</xsl:template>
	<xsl:template match="delegate" mode="keyword">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">Delegate</xsl:when>
			<xsl:when test="$lang='C#'">delegate</xsl:when>
			<xsl:when test="$lang='C++'">__delegate</xsl:when>
			<xsl:when test="$lang='JScript'">In JScript, you can use the delegates in the .NET Framework, but you cannot define your own.</xsl:when>
		</xsl:choose>	
		<xsl:text>&#160;</xsl:text>
	</xsl:template>	
	<xsl:template match="property" mode="keyword">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">Property&#160;</xsl:when>
			<xsl:when test="$lang='C#'"></xsl:when>
			<xsl:when test="$lang='C++'">__property&#160;</xsl:when>
			<xsl:when test="$lang='JScript'">function&#160;</xsl:when>
		</xsl:choose>	
	</xsl:template>
	
	<xsl:template match="@* | node() | text()" mode="gc-type"/>
	<xsl:template match="class | interface | delegate" mode="gc-type">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='C++'">__gc&#160;</xsl:when>
		</xsl:choose>	
	</xsl:template>
	<xsl:template match="structure | enumeration" mode="gc-type">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='C++'">__value&#160;</xsl:when>
		</xsl:choose>	
	</xsl:template>
	
	<xsl:template match="property" mode="property-name">
		<xsl:param name="lang"/>
		<xsl:param name="dir"/>

		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'"><xsl:value-of select="@name"/></xsl:when>
			<xsl:when test="$lang='C#'"><xsl:value-of select="@name"/></xsl:when>
			<xsl:when test="$lang='C++'"><xsl:value-of select="$dir"/>_<xsl:value-of select="@name"/></xsl:when>
			<xsl:when test="$lang='JScript'"><xsl:value-of select="$dir"/><xsl:text>&#160;</xsl:text><xsl:value-of select="@name"/></xsl:when>
		</xsl:choose>	
	</xsl:template>
	
	<xsl:template name="param-seperator">
		<xsl:param name="lang"/>
		
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'"><xsl:text>&#160;As&#160;</xsl:text></xsl:when>	
			<xsl:when test="$lang = 'JScript'">:<xsl:text>&#160;</xsl:text></xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="@* | node() | text()" mode="method-open"/>
	<xsl:template match="method" mode="method-open">
		<xsl:param name="lang"/>
		
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">
				<xsl:choose>
					<xsl:when test="@returnType = 'System.Void'">
						<xsl:text>Sub</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>Function</xsl:text>			
					</xsl:otherwise>
				</xsl:choose>
				<xsl:text>&#160;</xsl:text>
			</xsl:when>	
			<xsl:when test="$lang = 'JScript'">function<xsl:text>&#160;</xsl:text></xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="@* | node() | text()" mode="dir"/>
	<xsl:template match="parameter" mode="dir">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">ByVal&#160;</xsl:when>
		</xsl:choose>	
	</xsl:template>
	<xsl:template match="parameter[@direction='out']" mode="dir">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">ByRef&#160;</xsl:when>
			<xsl:when test="$lang='C#'">out&#160;</xsl:when>
			<xsl:when test="$lang='C++'">*&#160;</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="parameter[@direction='ref']" mode="dir">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">ByRef&#160;</xsl:when>
			<xsl:when test="$lang='C#'">ref&#160;</xsl:when>
			<xsl:when test="$lang='C++'">*&#160;</xsl:when>
		</xsl:choose>	
	</xsl:template>
		
	<xsl:template match="@* | node() | text()" mode="param-array"/>
	<xsl:template match="parameter[@isParamArray = 'true']" mode="param-array">
		<xsl:param name="lang"/>
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">ParamArray&#160;</xsl:when>
			<xsl:when test="$lang='C#'">params&#160;</xsl:when>
		</xsl:choose>	
	</xsl:template>


	<xsl:template name="lang-type">
		<xsl:param name="runtime-type" />
		<xsl:param name="lang"/>
		
		<xsl:variable name="old-type">
			<xsl:choose>
				<xsl:when test="contains($runtime-type, '[')">
					<xsl:value-of select="substring-before($runtime-type, '[')" />
				</xsl:when>
				<xsl:when test="contains($runtime-type, '&amp;')">
					<xsl:value-of select="substring-before($runtime-type, '&amp;')" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$runtime-type" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
	
		<xsl:choose>
			<xsl:when test="$lang='Visual Basic'">
				<xsl:variable name="new-type">
					<xsl:choose>
						<xsl:when test="$old-type='System.Byte'">Byte</xsl:when>
						<xsl:when test="$old-type='System.Int16'">Short</xsl:when>
						<xsl:when test="$old-type='System.Int32'">Integer</xsl:when>
						<xsl:when test="$old-type='System.Int64'">Long</xsl:when>
						<xsl:when test="$old-type='System.Single'">Single</xsl:when>
						<xsl:when test="$old-type='System.Double'">Double</xsl:when>
						<xsl:when test="$old-type='System.Decimal'">Decimal</xsl:when>
						<xsl:when test="$old-type='System.String'">String</xsl:when>
						<xsl:when test="$old-type='System.Char'">Char</xsl:when>
						<xsl:when test="$old-type='System.Boolean'">Boolean</xsl:when>
						<xsl:when test="$old-type='System.DateTime'">Date</xsl:when>
						<xsl:when test="$old-type='System.Object'">Object</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="strip-namespace">
								<xsl:with-param name="name" select="$old-type" />
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="contains($runtime-type, '[')">
						<xsl:value-of select="concat($new-type, '(', translate(substring-after($runtime-type, '['), ']', ')'))" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$new-type" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$lang='C#'">
				<xsl:variable name="new-type">
					<xsl:choose>
						<xsl:when test="$old-type='System.Byte'">byte</xsl:when>
						<xsl:when test="$old-type='Byte'">byte</xsl:when>
						<xsl:when test="$old-type='System.SByte'">sbyte</xsl:when>
						<xsl:when test="$old-type='SByte'">sbyte</xsl:when>
						<xsl:when test="$old-type='System.Int16'">short</xsl:when>
						<xsl:when test="$old-type='Int16'">short</xsl:when>
						<xsl:when test="$old-type='System.UInt16'">ushort</xsl:when>
						<xsl:when test="$old-type='UInt16'">ushort</xsl:when>
						<xsl:when test="$old-type='System.Int32'">int</xsl:when>
						<xsl:when test="$old-type='Int32'">int</xsl:when>
						<xsl:when test="$old-type='System.UInt32'">uint</xsl:when>
						<xsl:when test="$old-type='UInt32'">uint</xsl:when>
						<xsl:when test="$old-type='System.Int64'">long</xsl:when>
						<xsl:when test="$old-type='Int64'">long</xsl:when>
						<xsl:when test="$old-type='System.UInt64'">ulong</xsl:when>
						<xsl:when test="$old-type='UInt64'">ulong</xsl:when>
						<xsl:when test="$old-type='System.Single'">float</xsl:when>
						<xsl:when test="$old-type='Single'">float</xsl:when>
						<xsl:when test="$old-type='System.Double'">double</xsl:when>
						<xsl:when test="$old-type='Double'">double</xsl:when>
						<xsl:when test="$old-type='System.Decimal'">decimal</xsl:when>
						<xsl:when test="$old-type='Decimal'">decimal</xsl:when>
						<xsl:when test="$old-type='System.String'">string</xsl:when>
						<xsl:when test="$old-type='String'">string</xsl:when>
						<xsl:when test="$old-type='System.Char'">char</xsl:when>
						<xsl:when test="$old-type='Char'">char</xsl:when>
						<xsl:when test="$old-type='System.Boolean'">bool</xsl:when>
						<xsl:when test="$old-type='Boolean'">bool</xsl:when>
						<xsl:when test="$old-type='System.Void'">void</xsl:when>
						<xsl:when test="$old-type='Void'">void</xsl:when>
						<xsl:when test="$old-type='System.Object'">object</xsl:when>
						<xsl:when test="$old-type='Object'">object</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$old-type" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="contains($runtime-type, '[')">
						<xsl:value-of select="concat($new-type, '[', substring-after($runtime-type, '['))" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$new-type" />
					</xsl:otherwise>
				</xsl:choose>			
			</xsl:when>
			<xsl:when test="$lang='C++'">
				<xsl:variable name="new-type">
					<xsl:choose>
						<xsl:when test="$old-type='System.Byte'">byte</xsl:when>
						<xsl:when test="$old-type='Byte'">byte</xsl:when>
						<xsl:when test="$old-type='System.SByte'">sbyte</xsl:when>
						<xsl:when test="$old-type='SByte'">sbyte</xsl:when>
						<xsl:when test="$old-type='System.Int16'">short</xsl:when>
						<xsl:when test="$old-type='Int16'">short</xsl:when>
						<xsl:when test="$old-type='System.UInt16'">ushort</xsl:when>
						<xsl:when test="$old-type='UInt16'">ushort</xsl:when>
						<xsl:when test="$old-type='System.Int32'">int</xsl:when>
						<xsl:when test="$old-type='Int32'">int</xsl:when>
						<xsl:when test="$old-type='System.UInt32'">uint</xsl:when>
						<xsl:when test="$old-type='UInt32'">uint</xsl:when>
						<xsl:when test="$old-type='System.Int64'">long</xsl:when>
						<xsl:when test="$old-type='Int64'">long</xsl:when>
						<xsl:when test="$old-type='System.UInt64'">ulong</xsl:when>
						<xsl:when test="$old-type='UInt64'">ulong</xsl:when>
						<xsl:when test="$old-type='System.Single'">float</xsl:when>
						<xsl:when test="$old-type='Single'">float</xsl:when>
						<xsl:when test="$old-type='System.Double'">double</xsl:when>
						<xsl:when test="$old-type='Double'">double</xsl:when>
						<xsl:when test="$old-type='System.Decimal'">decimal</xsl:when>
						<xsl:when test="$old-type='Decimal'">decimal</xsl:when>
						<xsl:when test="$old-type='System.String'">String*</xsl:when>
						<xsl:when test="$old-type='String'">String*</xsl:when>
						<xsl:when test="$old-type='System.Char'">char</xsl:when>
						<xsl:when test="$old-type='Char'">char</xsl:when>
						<xsl:when test="$old-type='System.Boolean'">bool</xsl:when>
						<xsl:when test="$old-type='Boolean'">bool</xsl:when>
						<xsl:when test="$old-type='System.Void'">void</xsl:when>
						<xsl:when test="$old-type='Void'">void</xsl:when>
						<xsl:when test="$old-type='System.Object'">Object*</xsl:when>
						<xsl:when test="$old-type='Object'">Object*</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$old-type" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="contains($runtime-type, '[')">
						<xsl:value-of select="concat($new-type, '[', substring-after($runtime-type, '['))" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$new-type" />
					</xsl:otherwise>
				</xsl:choose>			
			</xsl:when>
			<xsl:when test="$lang='JScript'">
				<xsl:variable name="new-type">
					<xsl:choose>
						<xsl:when test="$old-type='System.Byte'">byte</xsl:when>
						<xsl:when test="$old-type='Byte'">byte</xsl:when>
						<xsl:when test="$old-type='System.SByte'">sbyte</xsl:when>
						<xsl:when test="$old-type='SByte'">sbyte</xsl:when>
						<xsl:when test="$old-type='System.Int16'">short</xsl:when>
						<xsl:when test="$old-type='Int16'">short</xsl:when>
						<xsl:when test="$old-type='System.UInt16'">ushort</xsl:when>
						<xsl:when test="$old-type='UInt16'">ushort</xsl:when>
						<xsl:when test="$old-type='System.Int32'">int</xsl:when>
						<xsl:when test="$old-type='Int32'">int</xsl:when>
						<xsl:when test="$old-type='System.UInt32'">uint</xsl:when>
						<xsl:when test="$old-type='UInt32'">uint</xsl:when>
						<xsl:when test="$old-type='System.Int64'">long</xsl:when>
						<xsl:when test="$old-type='Int64'">long</xsl:when>
						<xsl:when test="$old-type='System.UInt64'">ulong</xsl:when>
						<xsl:when test="$old-type='UInt64'">ulong</xsl:when>
						<xsl:when test="$old-type='System.Single'">float</xsl:when>
						<xsl:when test="$old-type='Single'">float</xsl:when>
						<xsl:when test="$old-type='System.Double'">double</xsl:when>
						<xsl:when test="$old-type='Double'">double</xsl:when>
						<xsl:when test="$old-type='System.Decimal'">decimal</xsl:when>
						<xsl:when test="$old-type='Decimal'">decimal</xsl:when>
						<xsl:when test="$old-type='System.String'">String</xsl:when>
						<xsl:when test="$old-type='String'">String</xsl:when>
						<xsl:when test="$old-type='System.Char'">char</xsl:when>
						<xsl:when test="$old-type='Char'">char</xsl:when>
						<xsl:when test="$old-type='System.Boolean'">bool</xsl:when>
						<xsl:when test="$old-type='Boolean'">bool</xsl:when>
						<xsl:when test="$old-type='System.Void'">void</xsl:when>
						<xsl:when test="$old-type='Void'">void</xsl:when>
						<xsl:when test="$old-type='System.Object'">Object</xsl:when>
						<xsl:when test="$old-type='Object'">Object</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$old-type" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="contains($runtime-type, '[')">
						<xsl:value-of select="concat($new-type, '[', substring-after($runtime-type, '['))" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$new-type" />
					</xsl:otherwise>
				</xsl:choose>			
			</xsl:when>
		</xsl:choose>				
	</xsl:template>	
</xsl:stylesheet>

  