<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<!-- -->
	<xsl:template match="/">
		<xsl:apply-templates select="ndoc/assembly/module/namespace/*[@id=$id]" />
	</xsl:template>
	<!-- -->
	<xsl:template match="class">
		<xsl:call-template name="type-members">
			<xsl:with-param name="type">Class</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<!-- -->
	<xsl:template match="interface">
		<xsl:call-template name="type-members">
			<xsl:with-param name="type">Interface</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<!-- -->
	<xsl:template match="structure">
		<xsl:call-template name="type-members">
			<xsl:with-param name="type">Structure</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<!-- -->
	<xsl:template name="get-big-member-plural">
		<xsl:param name="member" />
		<xsl:choose>
			<xsl:when test="$member='field'">Fields</xsl:when>
			<xsl:when test="$member='property'">Properties</xsl:when>
			<xsl:when test="$member='event'">Events</xsl:when>
			<xsl:when test="$member='operator'">Operators</xsl:when>
			<xsl:otherwise>Methods</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- -->
	<xsl:template name="get-small-member-plural">
		<xsl:param name="member" />
		<xsl:choose>
			<xsl:when test="$member='field'">fields</xsl:when>
			<xsl:when test="$member='property'">properties</xsl:when>
			<xsl:when test="$member='event'">events</xsl:when>
			<xsl:when test="$member='operator'">operators</xsl:when>
			<xsl:otherwise>methods</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- -->
	<xsl:template name="public-static-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Public' and @contract='Static']">
			<h4 class="dtH4">
				<xsl:text>Public Static (Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Public' and @contract='Static']">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="protected-static-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Family' and @contract='Static']">
			<h4 class="dtH4">
				<xsl:text>Protected Static (Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Family' and @contract='Static']">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="protected-internal-static-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='FamilyOrAssembly' and @contract='Static']">
			<h4 class="dtH4">
				<xsl:text>Protected Internal Static (Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='FamilyOrAssembly' and @contract='Static']">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="internal-static-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Assembly' and @contract='Static']">
			<h4 class="dtH4">
				<xsl:text>Internal Static (Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Assembly' and @contract='Static']">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="private-static-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Private' and @contract='Static']">
			<h4 class="dtH4">
				<xsl:text>Private Static (Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Private' and @contract='Static']">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="public-instance-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Public' and not(@contract='Static')]">
			<h4 class="dtH4">
				<xsl:text>Public Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Public' and not(@contract='Static')]">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="protected-instance-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Family' and not(@contract='Static')]">
			<h4 class="dtH4">
				<xsl:text>Protected Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Family' and not(@contract='Static')]">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="protected-internal-instance-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='FamilyOrAssembly' and not(@contract='Static')]">
			<h4 class="dtH4">
				<xsl:text>Protected Internal Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='FamilyOrAssembly' and not(@contract='Static')]">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="internal-instance-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Assembly' and not(@contract='Static')]">
			<h4 class="dtH4">
				<xsl:text>Internal Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Assembly' and not(@contract='Static')]">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="private-instance-section">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Private' and not(@contract='Static') and not(@interface)]">
			<h4 class="dtH4">
				<xsl:text>Private Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Private' and not(@contract='Static') and not(@interface)]">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template name="explicit-interface-implementations">
		<xsl:param name="member" />
		<xsl:if test="*[local-name()=$member and @access='Private' and not(@contract='Static') and @interface]">
			<h4 class="dtH4">
				<xsl:text>Explicit Interface Implementations</xsl:text>
			</h4>
			<div class="tablediv">
				<table class="dtTABLE" cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Private' and not(@contract='Static') and @interface]">
						<xsl:sort select="@name" />
					</xsl:apply-templates>
				</table>
			</div>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="property[@declaringType]">
		<xsl:variable name="name" select="@name" />
		<xsl:variable name="declaring-type-id" select="concat('T:', @declaringType)" />
		<xsl:text>&#10;</xsl:text>
		<tr VALIGN="top">
			<xsl:variable name="declaring-class" select="//class[@id=$declaring-type-id]" />
			<xsl:choose>
				<xsl:when test="$declaring-class">
					<td width="50%">
						<a>
							<xsl:attribute name="href">
								<xsl:call-template name="get-filename-for-property">
									<xsl:with-param name="property" select="$declaring-class/property[@name=$name]" />
								</xsl:call-template>
							</xsl:attribute>
							<xsl:value-of select="@name" />
						</a>
						<xsl:text> (inherited from </xsl:text>
						<b>
							<xsl:call-template name="get-datatype">
								<xsl:with-param name="datatype" select="@declaringType" />
							</xsl:call-template>
						</b>
						<xsl:text>)</xsl:text>
					</td>
					<td width="50%">
						<xsl:call-template name="summary-with-no-paragraph">
							<xsl:with-param name="member" select="//class[@id=$declaring-type-id]/property[@name=$name]" />
						</xsl:call-template>
					</td>
				</xsl:when>
				<xsl:otherwise>
					<td width="50%">
						<xsl:value-of select="@name" />
					</td>
					<td width="50%">
						<xsl:text>See the third party documentation for more information.</xsl:text>
					</td>
				</xsl:otherwise>
			</xsl:choose>
		</tr>
	</xsl:template>
	<!-- -->
	<xsl:template match="field[@declaringType]">
		<xsl:variable name="name" select="@name" />
		<xsl:variable name="declaring-type-id" select="concat('T:', @declaringType)" />
		<xsl:text>&#10;</xsl:text>
		<tr VALIGN="top">
			<xsl:variable name="declaring-class" select="//class[@id=$declaring-type-id]" />
			<xsl:choose>
				<xsl:when test="$declaring-class">
					<td width="50%">
						<a>
							<xsl:attribute name="href">
								<xsl:call-template name="get-filename-for-field">
									<xsl:with-param name="field" select="$declaring-class/field[@name=$name]" />
								</xsl:call-template>
							</xsl:attribute>
							<xsl:value-of select="@name" />
						</a>
						<xsl:text> (inherited from </xsl:text>
						<b>
							<xsl:call-template name="get-datatype">
								<xsl:with-param name="datatype" select="@declaringType" />
							</xsl:call-template>
						</b>
						<xsl:text>)</xsl:text>
					</td>
					<td width="50%">
						<xsl:call-template name="summary-with-no-paragraph">
							<xsl:with-param name="member" select="//class[@id=$declaring-type-id]/field[@name=$name]" />
						</xsl:call-template>
					</td>
				</xsl:when>
				<xsl:otherwise>
					<td width="50%">
						<xsl:value-of select="@name" />
					</td>
					<td width="50%">
						<xsl:text>See the third party documentation for more information.</xsl:text>
					</td>
				</xsl:otherwise>
			</xsl:choose>
		</tr>
	</xsl:template>
	<!-- -->
	<xsl:template match="property[@declaringType and starts-with(@declaringType, 'System.')]">
		<xsl:text>&#10;</xsl:text>
		<tr VALIGN="top">
			<td width="50%">
				<a>
					<xsl:attribute name="href">
						<xsl:call-template name="get-filename-for-system-property" />
					</xsl:attribute>
					<xsl:value-of select="@name" />
				</a>
				<xsl:text> (inherited from </xsl:text>
				<b>
					<xsl:value-of select="@declaringType" />
				</b>
				<xsl:text>)</xsl:text>
			</td>
			<td width="50%">
				<xsl:text>Select the method name to go to the Microsoft documentation.</xsl:text>
			</td>
		</tr>
	</xsl:template>
	<!-- -->
	<xsl:template match="method[@declaringType]">
		<xsl:variable name="name" select="@name" />
		<xsl:variable name="declaring-type-id" select="concat('T:', @declaringType)" />
		<xsl:if test="not(preceding-sibling::method[@name=$name])">
			<xsl:text>&#10;</xsl:text>
			<tr VALIGN="top">
				<xsl:variable name="declaring-class" select="//class[@id=$declaring-type-id]" />
				<xsl:choose>
					<xsl:when test="$declaring-class">
						<xsl:choose>
							<xsl:when test="following-sibling::method[@name=$name]">
								<td width="50%">
									<a>
										<xsl:attribute name="href">
											<xsl:call-template name="get-filename-for-inherited-method-overloads">
												<xsl:with-param name="declaring-type" select="@declaringType" />
												<xsl:with-param name="method-name" select="@name" />
											</xsl:call-template>
										</xsl:attribute>
										<xsl:if test="@interface">
											<xsl:call-template name="strip-namespace">
												<xsl:with-param name="name" select="@interface" />
											</xsl:call-template>
											<xsl:text>.</xsl:text>
										</xsl:if>
										<xsl:value-of select="@name" />
									</a>
									<xsl:text> (inherited from </xsl:text>
									<b>
										<xsl:call-template name="get-datatype">
											<xsl:with-param name="datatype" select="@declaringType" />
										</xsl:call-template>
									</b>
									<xsl:text>)</xsl:text>
								</td>
								<td width="50%">
									<xsl:text>Overloaded. </xsl:text>
									<xsl:call-template name="overloads-summary-with-no-paragraph">
										<xsl:with-param name="overloads" select="//class[@id=$declaring-type-id]/method[@name=$name]" />
									</xsl:call-template>
								</td>
							</xsl:when>
							<xsl:otherwise>
								<td width="50%">
									<a>
										<xsl:attribute name="href">
											<xsl:call-template name="get-filename-for-method">
												<xsl:with-param name="method" select="$declaring-class/method[@name=$name]" />
											</xsl:call-template>
										</xsl:attribute>
										<xsl:if test="@interface">
											<xsl:call-template name="strip-namespace">
												<xsl:with-param name="name" select="@interface" />
											</xsl:call-template>
											<xsl:text>.</xsl:text>
										</xsl:if>
										<xsl:value-of select="@name" />
									</a>
									<xsl:text> (inherited from </xsl:text>
									<b>
										<xsl:call-template name="get-datatype">
											<xsl:with-param name="datatype" select="@declaringType" />
										</xsl:call-template>
									</b>
									<xsl:text>)</xsl:text>
								</td>
								<td width="50%">
									<xsl:call-template name="summary-with-no-paragraph">
										<xsl:with-param name="member" select="//class[@id=$declaring-type-id]/method[@name=$name]" />
									</xsl:call-template>
								</td>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<td width="50%">
							<xsl:if test="@interface">
								<xsl:call-template name="strip-namespace">
									<xsl:with-param name="name" select="@interface" />
								</xsl:call-template>
								<xsl:text>.</xsl:text>
							</xsl:if>
							<xsl:value-of select="@name" />
						</td>
						<td width="50%">
							<xsl:text>See the third party documentation for more information.</xsl:text>
						</td>
					</xsl:otherwise>
				</xsl:choose>
			</tr>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="method[@declaringType and starts-with(@declaringType, 'System.')]">
		<xsl:text>&#10;</xsl:text>
		<tr VALIGN="top">
			<td width="50%">
				<a>
					<xsl:attribute name="href">
						<xsl:call-template name="get-filename-for-system-method" />
					</xsl:attribute>
					<xsl:if test="@interface">
						<xsl:call-template name="strip-namespace">
							<xsl:with-param name="name" select="@interface" />
						</xsl:call-template>
						<xsl:text>.</xsl:text>
					</xsl:if>
					<xsl:value-of select="@name" />
				</a>
				<xsl:text> (inherited from </xsl:text>
				<b>
					<xsl:call-template name="strip-namespace">
						<xsl:with-param name="name" select="@declaringType" />
					</xsl:call-template>
				</b>
				<xsl:text>)</xsl:text>
			</td>
			<td width="50%">
				<xsl:text>Select the method name to go to the Microsoft documentation.</xsl:text>
			</td>
		</tr>
	</xsl:template>
	<!-- -->
	<xsl:template match="field[not(@declaringType)]|property[not(@declaringType)]|event|method[not(@declaringType)]|operator">
		<xsl:variable name="member" select="local-name()" />
		<xsl:variable name="name" select="@name" />
		<xsl:variable name="contract" select="@contract" />
		<xsl:if test="not(preceding-sibling::*[local-name()=$member and @name=$name and (($contract='Static' and @contract='Static') or ($contract!='Static' and @contract!='Static'))])">
			<xsl:text>&#10;</xsl:text>
			<tr VALIGN="top">
				<xsl:choose>
					<xsl:when test="following-sibling::*[local-name()=$member and @name=$name and (($contract='Static' and @contract='Static') or ($contract!='Static' and @contract!='Static'))]">
						<td width="50%">
							<a>
								<xsl:attribute name="href">
									<xsl:call-template name="get-filename-for-individual-member-overloads">
										<xsl:with-param name="member">
											<xsl:value-of select="$member" />
										</xsl:with-param>
									</xsl:call-template>
								</xsl:attribute>
								<xsl:choose>
									<xsl:when test="local-name()='operator'">
										<xsl:call-template name="operator-name">
											<xsl:with-param name="name" select="@name" />
											<xsl:with-param name="from" select="parameter/@type"/>
											<xsl:with-param name="to" select="@returnType" />
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:if test="@interface">
											<xsl:call-template name="strip-namespace">
												<xsl:with-param name="name" select="@interface" />
											</xsl:call-template>
											<xsl:text>.</xsl:text>
										</xsl:if>
										<xsl:value-of select="@name" />
									</xsl:otherwise>
								</xsl:choose>
							</a>
						</td>
						<td width="50%">
							<xsl:text>Overloaded. </xsl:text>
							<xsl:call-template name="overloads-summary-with-no-paragraph" />
						</td>
					</xsl:when>
					<xsl:otherwise>
						<td width="50%">
							<a>
								<xsl:attribute name="href">
									<xsl:call-template name="get-filename-for-individual-member">
										<xsl:with-param name="member">
											<xsl:value-of select="$member" />
										</xsl:with-param>
									</xsl:call-template>
								</xsl:attribute>
								<xsl:choose>
									<xsl:when test="local-name()='operator'">
										<xsl:call-template name="operator-name">
											<xsl:with-param name="name" select="@name" />
											<xsl:with-param name="from" select="parameter/@type"/>
											<xsl:with-param name="to" select="@returnType" />
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:if test="@interface">
											<xsl:call-template name="strip-namespace">
												<xsl:with-param name="name" select="@interface" />
											</xsl:call-template>
											<xsl:text>.</xsl:text>
										</xsl:if>
										<xsl:value-of select="@name" />
									</xsl:otherwise>
								</xsl:choose>
							</a>
						</td>
						<td width="50%">
							<xsl:call-template name="summary-with-no-paragraph" />
						</td>
					</xsl:otherwise>
				</xsl:choose>
			</tr>
		</xsl:if>
	</xsl:template>
	<!-- -->
</xsl:stylesheet>
