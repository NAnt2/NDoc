﻿<?xml version="1.0" encoding="utf-8" ?>
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
			<h4>
				<xsl:text>Public Static (Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="table">
				<table cellspacing="0">
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
			<h4>
				<xsl:text>Protected Static (Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="table">
				<table cellspacing="0">
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
			<h4>
				<xsl:text>Protected Internal Static (Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="table">
				<table cellspacing="0">
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
			<h4>
				<xsl:text>Internal Static (Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="table">
				<table cellspacing="0">
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
			<h4>
				<xsl:text>Private Static (Shared) </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="table">
				<table cellspacing="0">
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
			<h4>
				<xsl:text>Public Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="table">
				<table cellspacing="0">
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
			<h4>
				<xsl:text>Protected Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="table">
				<table cellspacing="0">
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
			<h4>
				<xsl:text>Protected Internal Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="table">
				<table cellspacing="0">
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
			<h4>
				<xsl:text>Internal Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="table">
				<table cellspacing="0">
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
		<xsl:if test="*[local-name()=$member and @access='Private' and not(@contract='Static')]">
			<h4>
				<xsl:text>Private Instance </xsl:text>
				<xsl:call-template name="get-big-member-plural">
					<xsl:with-param name="member" select="$member" />
				</xsl:call-template>
			</h4>
			<div class="table">
				<table cellspacing="0">
					<xsl:apply-templates select="*[local-name()=$member and @access='Private' and not(@contract='Static')]">
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
						<xsl:apply-templates select="//class[@id=$declaring-type-id]/property[@name=$name]/documentation/summary/node()" mode="slashdoc" />
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
	<xsl:template match="method[@declaringType and not(@overload)]">
		<xsl:variable name="name" select="@name" />
		<xsl:variable name="declaring-type-id" select="concat('T:', @declaringType)" />
		<tr VALIGN="top">
			<xsl:variable name="declaring-class" select="//class[@id=$declaring-type-id]" />
			<xsl:choose>
				<xsl:when test="$declaring-class">
					<td width="50%">
						<a>
							<xsl:attribute name="href">
								<xsl:call-template name="get-filename-for-method">
									<xsl:with-param name="method" select="$declaring-class/method[@name=$name]" />
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
						<xsl:apply-templates select="//class[@id=$declaring-type-id]/method[@name=$name]/documentation/summary/node()" mode="slashdoc" />
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
	<xsl:template match="method[@declaringType and starts-with(@declaringType, 'System.')]">
		<tr VALIGN="top">
			<td width="50%">
				<a>
					<xsl:attribute name="href">
						<xsl:call-template name="get-filename-for-system-method" />
					</xsl:attribute>
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
	<xsl:template match="field|property[not(@declaringType)]|event|method[not(@declaringType)]|operator">
		<xsl:variable name="member" select="local-name()" />
		<xsl:variable name="name" select="@name" />
		<xsl:if test="not(preceding-sibling::*[local-name()=$member and @name=$name])">
			<tr VALIGN="top">
				<xsl:choose>
					<xsl:when test="following-sibling::*[local-name()=$member and @name=$name]">
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
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@name" />
									</xsl:otherwise>
								</xsl:choose>
							</a>
						</td>
						<td width="50%">Overloaded. <xsl:apply-templates select="documentation/summary/node()" mode="slashdoc" /></td>
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
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@name" />
									</xsl:otherwise>
								</xsl:choose>
							</a>
						</td>
						<td width="50%">
							<xsl:apply-templates select="documentation/summary/node()" mode="slashdoc" />
						</td>
					</xsl:otherwise>
				</xsl:choose>
			</tr>
		</xsl:if>
	</xsl:template>
	<!-- -->
</xsl:stylesheet>
